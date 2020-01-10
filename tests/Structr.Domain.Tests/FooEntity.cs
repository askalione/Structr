namespace Structr.Domain.Tests
{
    public class FooEntity : Entity<FooEntity, int>
    {
        public string Name { get; private set; }

        public FooEntity(int id)
        {
            Id = id;
            Name = GetType().Name + " " + id;
        }
    }
}
