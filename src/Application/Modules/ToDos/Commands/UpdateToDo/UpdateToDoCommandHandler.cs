using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;

namespace Application.Modules.ToDos.Commands.UpdateToDo;

public sealed class UpdateToDoCommandHandler(IUnitOfWork unitOfWork, IToDoRepository toDoRepository)
	: ICommandHandler<UpdateToDoCommand,Result>
{
	public async Task<Result> Handle(UpdateToDoCommand request, CancellationToken cancellationToken)
	{
		var toDo = await toDoRepository.GetToDoByIdAsync(request.Id);

		if (toDo is null)
		{
			return Result.Failure(ToDoErrors.ToDoNotFound(request.Id));
		}

		toDo.Name = request.Name;
		toDo.Description = request.Description;
		toDo.UpdatedAt = DateTime.Now;

		toDoRepository.UpdateToDo(toDo);

		await unitOfWork.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}