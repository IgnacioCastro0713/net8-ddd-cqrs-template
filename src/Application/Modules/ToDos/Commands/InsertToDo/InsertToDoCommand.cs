using Application.Abstractions.Messaging;

namespace Application.Modules.ToDos.Commands.InsertToDo;

public sealed record InsertToDoCommand(string Name, string Description) : ICommand<Result<ToDo>>;