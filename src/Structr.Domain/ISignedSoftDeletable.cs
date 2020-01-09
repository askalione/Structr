namespace Structr.Domain
{
    public interface ISignedSoftDeletable : ISoftDeletable
    {
        string DeletedBy { get; }
    }
}
