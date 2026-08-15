namespace Task_managment_system.Exceptions
{
    public class ConflictException : AppException
    {
        public ConflictException(string message) : base(message, 409) { }
    }
}
