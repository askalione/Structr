namespace Structr.Samples.Stateflows.Domain.BarEntity
{
    public class Bar : Entity
    {
        public EBarState State { get; private set; }
        public string Name { get; private set; }

        public Bar(string name)
        {
            Name = name;
            State = EBarState.Opened;
        }

        public void Edit(string name)
        {
            Name = name;
        }

        public void ChangeState(EBarState state)
        {
            State = state;
        }
    }
}
