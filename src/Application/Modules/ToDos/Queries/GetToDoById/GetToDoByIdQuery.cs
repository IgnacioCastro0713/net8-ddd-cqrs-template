using Application.Abstractions.Messaging;

namespace Application.Modules.ToDos.Queries.GetToDoById;

public sealed record GetToDoByIdQuery(int Id) : IQuery<Result<ToDo?>>;