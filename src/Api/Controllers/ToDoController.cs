using Application.Modules.ToDos.Commands.DeleteToDo;
using Application.Modules.ToDos.Commands.InsertToDo;
using Application.Modules.ToDos.Commands.UpdateToDo;
using Application.Modules.ToDos.Queries.GetToDoById;
using Application.Modules.ToDos.Queries.GetToDos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToDoController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await sender.Send(new GetToDosQuery());

        return result.ToOk();
    }

    [HttpGet(nameof(GetById))]
    public async Task<IActionResult> GetById([FromQuery] GetToDoByIdRequest request)
    {
        var result = await sender.Send(request.ToGetToDoByIdQuery());

        return result.ToOk();
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] InsertToDoRequest request)
    {
        var result = await sender.Send(request.ToInsertToDoCommand());

        return result.ToOk();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UpdateToDoRequest request)
    {
        var result = await sender.Send(request.ToUpdateToDoCommand());

        return result.ToOk();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] DeleteToDoRequest request)
    {
        var result = await sender.Send(request.ToDeleteToDoCommand());

        return result.ToOk();
    }
}