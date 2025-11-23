namespace Server.Application.Exeptions
{
    public class NotFoundExeption : Exception
    {
        private const string DefaultMessage = "Resource Not Found";

        public NotFoundExeption()
        {
        }

        public NotFoundExeption(string message = DefaultMessage)
            : base(message)
        {
        }
    }
}