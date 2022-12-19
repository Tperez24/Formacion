namespace PatronesDeComportamiento.State
{
    public abstract class MovementInputState
    {
        protected MovementInput MovementInput;
        public void SetContext(MovementInput movementInput) => MovementInput = movementInput;
        public abstract void Move();
        public abstract void Transition();
    }
}