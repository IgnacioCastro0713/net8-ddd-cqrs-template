using System.Net;

namespace Domain.Common;

public sealed record Error(string Code, string Description, HttpStatusCode StatusCode)
{
	public static readonly Error None = new(string.Empty, string.Empty, HttpStatusCode.OK);

	public static Error BadRequest(string code, string description)
	{
		return new Error(code, description, HttpStatusCode.BadRequest);
	}

	public static Error NotFound(string code, string description)
	{
		return new Error(code, description, HttpStatusCode.NotFound);
	}

	public static Error Conflict(string code, string description)
	{
		return new Error(code, description, HttpStatusCode.Conflict);
	}
}