namespace Server.Domain.Exeptions
{
    public class DomainExeption : Exception
    {
        private const string DefaultMessage = "A domain error has occurred.";

        public DomainExeption()
        {
        }

        public DomainExeption(string message = DefaultMessage)
            : base(message)
        {
        }
    }
}