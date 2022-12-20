namespace Demo.InputState
{
    public class MovementInput
    {
        private InputState _inputState;

        public MovementInput(InputState state) => TransitionTo(state);

        public void TransitionTo(InputState state)
        {
            _inputState = state;
            _inputState.SetContext(this);
        }

        public void StateAction() => _inputState.Move();
        
        public void StateTransition(InputState state) => _inputState.TransitionToNewState(state);
    }
}