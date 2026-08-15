namespace Task_managment_system.Exceptions
{
    public class ValidationException : AppException
    {
        public ValidationException(string message) : base (message, 422) { }
    }
}
