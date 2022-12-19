using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PatronesDeComportamiento.State
{
    public class InputController : MonoBehaviour
    {
        private MovementInput _movementInput;
        private MovementInputState _movementInputState;
        private InputAction _moveAction;
        
        public MovementState movementState;

        public enum MovementState
        {
            EnemyMove,
            InterfaceMove
        }
        private void Start()
        {
            _movementInputState = new MoveEnemyState();
            _movementInput = new MovementInput(GetState());
        }

        private void OnEnable()
        {
            var master = new Master();
            master.Enable();
            _moveAction = master.PlayerInputKeyboard.Movement;
            _moveAction.performed += Move;
        }

        private void Move(InputAction.CallbackContext obj) => _movementInputState.Move();

        private MovementInputState GetState()
        {
            return movementState switch
            {
                MovementState.EnemyMove => new MoveEnemyState(),
                MovementState.InterfaceMove => new MoveOnInterfaceState(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}