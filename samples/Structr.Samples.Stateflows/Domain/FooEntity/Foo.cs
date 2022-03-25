namespace Structr.Samples.Stateflows.Domain.FooEntity
{
    public class Foo : Entity
    {
        public FooState State { get; private set; }
        public string Email { get; private set; }

        public Foo(string email) : base()
        {
            Email = email;
            State = FooState.Unsent;
        }

        public void Edit(string email)
        {
            Email = email;
        }

        public void ChangeState(FooState state)
        {
            State = state;
        }
    }
}
