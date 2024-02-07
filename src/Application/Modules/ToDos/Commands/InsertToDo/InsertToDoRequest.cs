namespace Application.Modules.ToDos.Commands.InsertToDo;

public sealed record InsertToDoRequest(string Name, string Description)
{
	public InsertToDoCommand ToInsertToDoCommand()
	{
		return new InsertToDoCommand(
			Name,
			Description
		);
	}
}