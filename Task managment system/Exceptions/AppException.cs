namespace Task_managment_system.Exceptions
{
    public class AppException : Exception
    {
        int StatusCode { get; init; }
        public AppException(string message, int statusCode) : base (message) { StatusCode = statusCode; }
    }
}
