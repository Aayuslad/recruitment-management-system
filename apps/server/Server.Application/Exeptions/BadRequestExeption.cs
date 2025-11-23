namespace Server.Application.Exeptions
{
    public class BadRequestExeption : Exception
    {
        private const string DefaultMessage = "Bad Request";

        public BadRequestExeption()
        {
        }

        public BadRequestExeption(string message = DefaultMessage)
            : base(message)
        {
        }
    }
}