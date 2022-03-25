namespace Structr.Samples.Stateflows.Domain.BarEntity
{
    public class Bar : Entity
    {
        public BarState State { get; private set; }
        public string Name { get; private set; }

        public Bar(string name)
        {
            Name = name;
            State = BarState.Opened;
        }

        public void Edit(string name)
        {
            Name = name;
        }

        public void ChangeState(BarState state)
        {
            State = state;
        }
    }
}
