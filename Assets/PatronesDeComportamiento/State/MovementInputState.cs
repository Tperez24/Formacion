namespace PatronesDeComportamiento.State
{
    public abstract class MovementInputState
    {
        protected MovementInput _movementInput;
        public void SetContext(MovementInput movementInput) => _movementInput = movementInput;
        public abstract void Move();
        public abstract void Transition();
    }
}