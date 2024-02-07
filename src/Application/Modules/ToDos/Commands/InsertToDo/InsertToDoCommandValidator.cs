namespace Application.Modules.ToDos.Commands.InsertToDo;

public sealed class InsertToDoCommandValidator : AbstractValidator<InsertToDoCommand>
{
	public InsertToDoCommandValidator()
	{
		RuleFor(c => c.Name)
			.NotEmpty()
			.MaximumLength(50);

		RuleFor(c => c.Description)
			.NotEmpty()
			.MinimumLength(5)
			.MaximumLength(50);
	}
}