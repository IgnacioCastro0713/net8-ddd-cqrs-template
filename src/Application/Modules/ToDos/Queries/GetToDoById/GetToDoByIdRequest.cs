namespace Application.Modules.ToDos.Queries.GetToDoById;

public sealed record GetToDoByIdRequest(int Id)
{
	public GetToDoByIdQuery ToGetToDoByIdQuery()
	{
		return new GetToDoByIdQuery(Id);
	}
}