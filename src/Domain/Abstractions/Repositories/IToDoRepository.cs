namespace Domain.Abstractions.Repositories;

public interface IToDoRepository
{
	public Task<IEnumerable<ToDo>> GetToDosAsync();
	public Task<ToDo?> GetToDoByIdAsync(int id);
	public void InsertToDo(ToDo toDo);
	public void UpdateToDo(ToDo toDo);
	public void DeleteToDo(ToDo toDo);
}