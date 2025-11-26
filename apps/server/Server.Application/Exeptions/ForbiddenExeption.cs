namespace Server.Application.Exeptions
{
    public class ForbiddenExeption : Exception
    {
        private const string DefaultMessage = "Forbidden";

        public ForbiddenExeption()
        {
        }

        public ForbiddenExeption(string message = DefaultMessage)
            : base(message)
        {
        }
    }
}