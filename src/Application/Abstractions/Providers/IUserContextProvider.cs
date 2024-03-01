namespace Application.Abstractions.Providers;

public interface IUserContextProvider
{
    string? NtUser { get; }
    int? Id { get; }
}