namespace Server.Application.Exeptions
{
    public class UnAuthorisedExeption : Exception
    {
        private const string DefaultMessage = "Unauthorized Access";

        public UnAuthorisedExeption()
        {
        }

        public UnAuthorisedExeption(string message = DefaultMessage)
            : base(message)
        {
        }
    }
}