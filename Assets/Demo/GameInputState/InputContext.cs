namespace Demo.GameInputState
{
    public class InputContext
    {
        private InputState _inputState;

        public InputContext(InputState inputState) => _inputState = inputState;

        public void TransitionTo(InputState state)
        {
            _inputState = state;
            _inputState.SetContext(this);
        }

        public void MoveAction() => _inputState.Move();
        
        public void StateTransition(InputState state) => _inputState.TransitionTo(state);
    }
}