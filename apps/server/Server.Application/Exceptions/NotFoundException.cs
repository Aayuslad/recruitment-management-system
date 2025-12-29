namespace Server.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        private const string DefaultMessage = "Resource Not Found";

        public NotFoundException()
        {
        }

        public NotFoundException(string message = DefaultMessage)
            : base(message)
        {
        }
    }
}