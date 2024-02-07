namespace Application.Modules.ToDos.Commands.DeleteToDo;

public sealed record DeleteToDoRequest(int Id)
{
	public DeleteToDoCommand ToDeleteToDoCommand()
	{
		return new DeleteToDoCommand(Id);
	}
}