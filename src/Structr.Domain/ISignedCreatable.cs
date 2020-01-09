namespace Structr.Domain
{
    public interface ISignedCreatable : ICreatable
    {
        string CreatedBy { get; }
    }
}
