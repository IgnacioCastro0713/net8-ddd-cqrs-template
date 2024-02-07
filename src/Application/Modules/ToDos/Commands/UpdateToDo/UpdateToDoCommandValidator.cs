namespace Application.Modules.ToDos.Commands.UpdateToDo;

public sealed class UpdateToDoCommandValidator : AbstractValidator<UpdateToDoCommand>
{
	public UpdateToDoCommandValidator()
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