namespace Server.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        private const string DefaultMessage = "Bad Request";

        public BadRequestException()
        {
        }

        public BadRequestException(string message = DefaultMessage)
            : base(message)
        {
        }
    }
}