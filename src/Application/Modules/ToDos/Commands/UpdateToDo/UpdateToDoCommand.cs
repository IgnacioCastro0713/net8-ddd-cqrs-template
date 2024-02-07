using Application.Abstractions.Messaging;

namespace Application.Modules.ToDos.Commands.UpdateToDo;

public sealed record UpdateToDoCommand(
	int Id,
	string Name,
	string Description) : ICommand<Result>;