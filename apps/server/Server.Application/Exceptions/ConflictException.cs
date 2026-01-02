namespace Server.Application.Exceptions
{
    public class ConflictException : Exception
    {
        private const string DefaultMessage = "Conflict Occurred";

        public ConflictException()
        {
        }

        public ConflictException(string message = DefaultMessage)
            : base(message)
        {
        }
    }
}