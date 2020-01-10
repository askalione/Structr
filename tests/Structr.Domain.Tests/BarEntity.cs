namespace Structr.Domain.Tests
{
    public class BarEntity : Entity<FooEntity, int>
    {
        public EType Type { get; private set; }

        public BarEntity(int id, EType type)
        {
            Id = id;
            Type = type;
        }
    }
}
