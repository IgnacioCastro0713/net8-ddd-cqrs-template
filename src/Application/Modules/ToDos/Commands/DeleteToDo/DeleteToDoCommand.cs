using Application.Abstractions.Messaging;

namespace Application.Modules.ToDos.Commands.DeleteToDo;

public sealed record DeleteToDoCommand(int Id) : ICommand<Result>;