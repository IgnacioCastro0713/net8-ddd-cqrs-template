using Application.Extensions;
using Application.Modules.Users.Commands.DeleteUser;
using Application.Modules.Users.Commands.InsertUser;
using Application.Modules.Users.Commands.UpdateUser;
using Application.Modules.Users.Queries.GetAllUsers;
using Application.Modules.Users.Queries.GetUserById;

namespace Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await sender.Send(new GetAllUsersQuery());

        return result.ToOk();
    }

    [HttpGet("GetById/{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await sender.Send(new GetUserByIdQuery(id));

        return result.ToOk();
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] InsertUserRequest request)
    {
        var command = request.Adapt<InsertUserCommand>();

        var result = await sender.Send(command);

        return result.ToOk();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UpdateUserRequest request)
    {
        var command = request.Adapt<UpdateUserCommand>();

        var result = await sender.Send(command);

        return result.ToOk();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var result = await sender.Send(new DeleteUserCommand(id));

        return result.ToOk();
    }
}