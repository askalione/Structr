namespace Structr.Samples.Stateflows.Domain.FooEntity
{
    public class Foo : Entity
    {
        public EFooState State { get; private set; }
        public string Email { get; private set; }

        public Foo(string email) : base()
        {
            Email = email;
            State = EFooState.Unsent;
        }

        public void Edit(string email)
        {
            Email = email;
        }

        public void ChangeState(EFooState state)
        {
            State = state;
        }
    }
}
