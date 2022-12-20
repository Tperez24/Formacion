namespace Demo.InputState
{
    public abstract class InputState
    {
        protected MovementInput Context;
        public void SetContext(MovementInput movementContext) => Context = movementContext;
        public abstract void Move();
        public abstract void TransitionToNewState(InputState state);
    }
}