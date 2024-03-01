using Domain.Shared;

namespace Domain.Errors;

public static class UserErrors
{
	public static Error UserNotFound(object? value) =>
		Error.NotFound(
			"User.UserNotFound",
			$"The user was not found with the value: {value}");
}