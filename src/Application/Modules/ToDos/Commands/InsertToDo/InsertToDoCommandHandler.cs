using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;

namespace Application.Modules.ToDos.Commands.InsertToDo;

public sealed class InsertToDoCommandHandler(
	IUnitOfWork unitOfWork,
	IToDoRepository toDoRepository) : ICommandHandler<InsertToDoCommand, Result<ToDo>>
{
	public async Task<Result<ToDo>> Handle(InsertToDoCommand request, CancellationToken cancellationToken)
	{
		var toDo = new ToDo
		{
			Name = request.Name,
			Description = request.Description,
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now
		};

		toDoRepository.InsertToDo(toDo);

		await unitOfWork.SaveChangesAsync(cancellationToken);

		return toDo;
	}
}