namespace Structr.Domain
{
    public interface ISignedModifiable : IModifiable
    {
        string ModifiedBy { get; }
    }
}
