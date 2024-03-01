using Domain.Dtos.Authentication;

namespace Application.Modules.Authentication.Queries.Authentication;

public sealed record AuthenticationResponse(AuthenticatedUserResponseDto User, string Token, DateTime Expires);