namespace Server.Core.Entities
{
    /// <summary>
    /// Generic entity interface with a GUID identifier.
    /// </summary>
    public interface IEntity<Guid>
    {
        Guid Id { get; }
    }
}