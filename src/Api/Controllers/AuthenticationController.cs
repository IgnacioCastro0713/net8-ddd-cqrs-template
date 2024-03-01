using Application.Extensions;
using Application.Modules.Authentication.Queries.Authentication;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(ISender sender) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> Post(AuthenticationRequest request)
	{
		var command = request.Adapt<AuthenticationCommand>();

		var result = await sender.Send(command);

		return result.ToOk();
	}
}