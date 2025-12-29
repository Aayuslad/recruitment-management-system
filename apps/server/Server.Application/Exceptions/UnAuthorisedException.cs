namespace Server.Application.Exceptions
{
    public class UnAuthorisedException : Exception
    {
        private const string DefaultMessage = "Unauthorized Access";

        public UnAuthorisedException()
        {
        }

        public UnAuthorisedException(string message = DefaultMessage)
            : base(message)
        {
        }
    }
}