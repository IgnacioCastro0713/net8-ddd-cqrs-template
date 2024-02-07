using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;

namespace Application.Modules.ToDos.Queries.GetToDos;

public sealed class GetToDosQueryHandler(IToDoRepository toDoRepository)
    : IQueryHandler<GetToDosQuery, Result<IEnumerable<ToDo>>>
{
    public async Task<Result<IEnumerable<ToDo>>> Handle(GetToDosQuery query, CancellationToken cancellationToken)
    {
        return (await toDoRepository.GetToDosAsync()).ToList();
    }
}