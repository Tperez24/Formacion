using Demo.Input_Adapter;
using Demo.Scripts.StaticClasses;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Demo.GameInputState
{
    public class InputController : MonoBehaviour
    {
        private IInput Input { get; set; }
        
        private InputContext _inputContext;
        private InputState _inputState;
        
        private InputAction _moveAction,_attackAction,_specialAction;

        public void SetInput(IInput input) => Input = input;
        public void Initialize()
        {
            var master = new Master();
            master.Enable();
            
            _moveAction = Input.GetInput().Find(input => input.name == ActionNames.Movement());
            _attackAction = Input.GetInput().Find(input => input.name == ActionNames.Attack());
            _specialAction = Input.GetInput().Find(input => input.name == ActionNames.Special());
            
            SubscribeToInputs();
            
            _inputState = new PlayerInputState();
            _inputContext = new InputContext(_inputState);
            
            _inputState.SetContext(_inputContext);
        }

        private void SubscribeToInputs()
        {
            _moveAction.performed += Move;
            _attackAction.performed += Attack;
            _specialAction.performed += SpecialAttack;
            _specialAction.started += AimSpecialAttack;
            _specialAction.canceled += CancelSpecialAttack;
        }

        private void UnsubscribeToInputs()
        {
            _moveAction.performed -= Move;
            _attackAction.performed -= Attack;
            _specialAction.performed -= SpecialAttack;
            _specialAction.started -= AimSpecialAttack;
            _specialAction.canceled -= CancelSpecialAttack;
        }
        
        private void AimSpecialAttack(InputAction.CallbackContext obj) => _inputState.ChargeSpecialAttack();

        private void CancelSpecialAttack(InputAction.CallbackContext obj) => _inputState.CancelSpecialAttack();

        private void SpecialAttack(InputAction.CallbackContext obj) => _inputState.LaunchSpecialAttack();

        private void Attack(InputAction.CallbackContext obj) => _inputState.PressAttack();

        private void Move(InputAction.CallbackContext obj) => _inputState.Move();
    }
}