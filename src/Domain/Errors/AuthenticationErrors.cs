using Domain.Shared;

namespace Domain.Errors;

public static class AuthenticationErrors
{
    public static readonly Error UserActiveDirectoryNotFound =
        Error.NotFound(
            "Authentication.UserActiveDirectoryNotFound",
            "The current user was not found in Active directory.");

    public static readonly Error UserActiveDirectoryInvalidCredential =
        Error.Validation(
            "Authentication.UserActiveDirectoryInvalidCredential",
            "Wrong Credentials invalid username or password.");
}