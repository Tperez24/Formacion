namespace Demo.InputState
{
    public class PlayerMovementState : InputState
    {
        public override void Move()
        {
            throw new System.NotImplementedException();
        }

        public override void TransitionToNewState(InputState state) => Context.TransitionTo(state);
    }
}