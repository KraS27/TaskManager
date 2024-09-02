namespace TaskManager.Entities.Exceptions
{
    public class PaginationException : Exception
    {
        public PaginationException() { }
        public PaginationException(string message) : base(message) { }
    }
}
