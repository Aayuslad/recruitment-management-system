namespace Server.Application.Exeptions
{
    public class ConflictExeption : Exception
    {
        private const string DefaultMessage = "Conflict Occurred";

        public ConflictExeption()
        {
        }

        public ConflictExeption(string message = DefaultMessage)
            : base(message)
        {
        }
    }
}