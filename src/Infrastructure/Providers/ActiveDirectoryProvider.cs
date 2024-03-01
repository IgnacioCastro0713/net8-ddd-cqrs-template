using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using Domain.Dtos.ActiveDirectory;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Providers;

public sealed class ActiveDirectoryProvider(ILogger<ActiveDirectoryProvider> logger) : IActiveDirectoryProvider
{
    private const char Separator = '\\';

    public Result<UserPrincipalResponseDto> FindByIdentity(string? identityValue)
    {
        try
        {
            var ntUser = identityValue ?? string.Empty;
            var userPrincipal = FindUserInActiveDirectory(ntUser);

            return userPrincipal is not null
                ? userPrincipal
                : AuthenticationErrors.UserActiveDirectoryNotFound;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Active Directory Error");
            return AuthenticationErrors.UserActiveDirectoryNotFound;
        }
    }

    private static UserPrincipalResponseDto? FindUserInActiveDirectory(string ntUser)
    {
        const string path = "GC://bosch.com";
        var username = ntUser.Split(Separator).Last();
        var filter = $"(&(ObjectClass=person)(SAMAccountName={username}))";

        using var searcher = new DirectorySearcher(new DirectoryEntry(path));
        searcher.Filter = filter;

        var directoryEntry = searcher.FindOne()?.GetDirectoryEntry();
        var properties = directoryEntry?.Properties;

        return properties is not null
            ? new UserPrincipalResponseDto
            {
                Name = properties["Name"].Value?.ToString() ?? string.Empty,
                DisplayName = properties["DisplayName"].Value?.ToString() ?? string.Empty
            }
            : null;
    }

    public bool ValidateCredentials(string ntUser, string password)
    {
        using var context = GetPrincipalContextByNtUSer(ntUser);
        var username = ntUser.Split(Separator).Last();

        return context is not null && context.ValidateCredentials(username, password);
    }

    private static PrincipalContext? GetPrincipalContextByNtUSer(string ntUser)
    {
        var domains = GetDomains(ntUser);
        PrincipalContext? context = null;

        foreach (var domain in domains)
        {
            context = new PrincipalContext(ContextType.Domain, domain);
            var searcher = new PrincipalSearcher(new UserPrincipal(context) { SamAccountName = ntUser });

            if (searcher.FindOne() is not UserPrincipal) continue;

            return context;
        }

        return context;
    }

    private static IEnumerable<string> GetDomains(string ntUser)
    {
        var domains = Forest
            .GetCurrentForest()
            .Domains
            .Cast<System.DirectoryServices.ActiveDirectory.Domain>()
            .Select(d => d.Name);

        var domain = ntUser.Split(Separator).First();

        return !ntUser.Contains(Separator)
            ? domains
            : domains.Where(s => s.Contains(domain, StringComparison.CurrentCultureIgnoreCase));
    }
}