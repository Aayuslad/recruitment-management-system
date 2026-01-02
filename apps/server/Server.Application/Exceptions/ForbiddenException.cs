namespace Server.Application.Exceptions
{
    public class ForbiddenException : Exception
    {
        private const string DefaultMessage = "Forbidden";

        public ForbiddenException()
        {
        }

        public ForbiddenException(string message = DefaultMessage)
            : base(message)
        {
        }
    }
}