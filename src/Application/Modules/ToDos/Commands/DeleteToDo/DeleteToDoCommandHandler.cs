using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;

namespace Application.Modules.ToDos.Commands.DeleteToDo;

public sealed class DeleteToDoCommandHandler(
    IUnitOfWork unitOfWork,
    IToDoRepository toDoRepository) : ICommandHandler<DeleteToDoCommand, Result>
{
    public async Task<Result> Handle(DeleteToDoCommand request, CancellationToken cancellationToken)
    {
        var toDo = await toDoRepository.GetToDoByIdAsync(request.Id);
        
        if (toDo is null)
        {
            return Result.Failure(ToDoErrors.ToDoNotFound(request.Id));
        }

        toDoRepository.DeleteToDo(toDo);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}