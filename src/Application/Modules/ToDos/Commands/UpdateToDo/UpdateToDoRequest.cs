namespace Application.Modules.ToDos.Commands.UpdateToDo;

public sealed record UpdateToDoRequest(int Id, string Name, string Description)
{
	public UpdateToDoCommand ToUpdateToDoCommand()
	{
		return new UpdateToDoCommand(
			Id,
			Name,
			Description
		);
	}
}