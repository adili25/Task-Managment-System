namespace Task_managment_system.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message) : base (message, 404) { }
    }
}
