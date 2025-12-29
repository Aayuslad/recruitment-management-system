namespace Server.Domain.Exceptions
{
    public class DomainException : Exception
    {
        private const string DefaultMessage = "A domain error has occurred.";

        public DomainException()
        {
        }

        public DomainException(string message = DefaultMessage)
            : base(message)
        {
        }
    }
}