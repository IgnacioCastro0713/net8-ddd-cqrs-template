using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;

namespace Application.Modules.ToDos.Queries.GetToDoById;

public sealed class GetToDoByIdQueryHandler(IToDoRepository toDoRepository)
	: IQueryHandler<GetToDoByIdQuery, Result<ToDo?>>
{
	public async Task<Result<ToDo?>> Handle(GetToDoByIdQuery query, CancellationToken cancellationToken)
	{
		var toDo = await toDoRepository.GetToDoByIdAsync(query.Id);

		if (toDo is null) return Result.Failure(ToDoErrors.ToDoNotFound(query.Id));

		return toDo;
	}
}