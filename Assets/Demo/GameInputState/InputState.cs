namespace Demo.GameInputState
{
    public abstract class InputState
    {
        protected InputContext InputContext;
        public void SetContext(InputContext inputContext) => InputContext = inputContext;
        public abstract void Move();
        public abstract void PressAttack();
        public abstract void ChargeSpecialAttack();
        public abstract void LaunchSpecialAttack();
        public abstract void CancelSpecialAttack();
        public abstract void TransitionTo(InputState state);
    }
}