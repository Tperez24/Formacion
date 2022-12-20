using UnityEngine;

namespace Demo.InputState
{
    public class InputStateController : MonoBehaviour
    {
        private MovementInput _movementInput;
        private PlayerMovementState _movementInputState;
        
        private void Start()
        {
            _movementInputState = new PlayerMovementState();
            _movementInput = new MovementInput(_movementInputState);
            _movementInputState.SetContext(_movementInput);
        }

    }
}