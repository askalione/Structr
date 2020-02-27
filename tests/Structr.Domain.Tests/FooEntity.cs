namespace Structr.Domain.Tests
{
    public class FooEntity : Entity<FooEntity, object>
    {
        public string Name { get; private set; }

        public FooEntity(object id)
        {
            Id = id;
            Name = GetType().Name + " " + id;
        }
    }
}
