namespace Domain.Entities;

public class ToDo : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
}