using Application.Abstractions.Messaging;

namespace Application.Modules.ToDos.Queries.GetToDos;

public sealed record GetToDosQuery : IQuery<Result<IEnumerable<ToDo>>>;