namespace Domain.Errors;

public static class ToDoErrors
{
    public static Error ToDoOtherError = Error.BadRequest("ToDo.OtherError", "Description other error");

    public static Error ToDoNotFound(object value) =>
        Error.NotFound("ToDo.NotFound", $"The ToDo with value '{value}' was not found");
}