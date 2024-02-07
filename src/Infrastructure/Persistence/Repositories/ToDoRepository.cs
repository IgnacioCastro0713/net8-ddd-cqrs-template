using Domain.Abstractions.Repositories;

namespace Infrastructure.Persistence.Repositories;

public sealed class ToDoRepository(ApplicationDbContext context) : IToDoRepository
{
	public async Task<IEnumerable<ToDo>> GetToDosAsync()
	{
		return await context.ToDos.AsNoTracking().ToListAsync();
	}

	public async Task<ToDo?> GetToDoByIdAsync(int id)
	{
		return await context.ToDos.FindAsync(id);
	}

	public void InsertToDo(ToDo toDo)
	{
		context.ToDos.Add(toDo);
	}

	public void UpdateToDo(ToDo toDo)
	{
		context.ToDos.Update(toDo);
	}

	public void DeleteToDo(ToDo toDo)
	{
		context.ToDos.Remove(toDo);
	}
}