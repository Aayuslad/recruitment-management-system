namespace Server.Application.Abstractions.Services
{
    public interface IUserContext
    {
        Guid AuthId { get; }
        Guid UserId { get; }
        string? UserName { get; }
    }
}