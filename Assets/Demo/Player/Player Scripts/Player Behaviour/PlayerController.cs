using System;
using Demo.Input_Adapter;
using Demo.Player.Player_Scripts.Player_Behaviour;
using Demo.Projectile_Abstract_Factory;
using Demo.Scripts.StaticClasses;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player.Player_Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private IAbstractPointer pointer;
        private IAbstractSpell spell;
        private IInput Input { get; set; }
        private Animator _playerAnimator;
        private InputAction _moveAction,_attackAction,_specialAction;
        private Vector2 _direction,_lastDirection;
        private object _lastValueGiven;
        private Rigidbody2D _rigidbody;
       
        private float _speed = 2f;
        private readonly UnityEvent _onDirectionChanged = new UnityEvent();
        
        public void SetInput(IInput input) => Input = input;

        private Vector2 Direction
        {
            get => _direction;
            set
            {
                if (_direction == value) return;
                _direction = value;
                _onDirectionChanged?.Invoke();
            } 
        }

        public void Initialize()
        {
            Getters();
            SetActions();
            SubscribeToInputs();
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            _moveAction.performed -= ChangeDirection;
            _attackAction.performed -= Attack;
            _specialAction.performed -= LaunchSpecialAttack;
        }

        private void Getters()
        {
            _playerAnimator = GetComponentInParent<Animator>();
            _rigidbody = GetComponentInParent<Rigidbody2D>();
        }
        private void SetActions()
        {
            _moveAction = Input.GetInput().Find(input => input.name == ActionNames.Movement());
            _attackAction = Input.GetInput().Find(input => input.name == ActionNames.Attack());
            _specialAction = Input.GetInput().Find(input => input.name == ActionNames.Special());
        }

        private void SubscribeToInputs()
        {
            _moveAction.performed += ChangeDirection;
            _attackAction.performed += Attack;
            _specialAction.performed += LaunchSpecialAttack;
            _specialAction.started += AimSpecialAttack;
        }
        private void SubscribeToEvents() => _onDirectionChanged.AddListener(MovePlayer);

        private void ChangeDirection(InputAction.CallbackContext context)
        {
            var dir = context.ReadValue<Vector2>();

            if (dir == Vector2.zero)
            {
                StopPlayer();
                return;
            }
            
            SetDirectionNotifying(GetDirectionWithoutDiagonal(dir));
        }

        private Vector2 GetDirectionWithoutDiagonal(Vector2 dir) => Mathf.Abs(dir.x) > Mathf.Abs(dir.y) ? new Vector2(dir.x, 0).normalized : new Vector2(0, dir.y).normalized;

        private void MovePlayer()
        {
            ManageAnimation(AnimationNames.Horizontal(),Direction.x);
            ManageAnimation(AnimationNames.Vertical(),Direction.y);
            ManageAnimation(AnimationNames.IsMoving(),true);

            UpdatePosition();
        }

        private void UpdatePosition()
        {
            var actualDir = new Vector2(Direction.x, Direction.y);
            if(_lastDirection == actualDir) return;
            SetVelocity(actualDir);
        }

        private void StopPlayer()
        {
            SetDirectionWithoutNotify(Vector2.zero);
            ManageAnimation(AnimationNames.IsMoving(), false);
            SetVelocity(Vector2.zero);
        }
        private void ManageAnimation(string animationName, object type)
        {
            if (_lastValueGiven == type) return;
            _lastValueGiven = type;

            AnimationAction(animationName, type).Invoke();
        }
        
        private void Attack(InputAction.CallbackContext context) => AnimationAction(AnimationNames.IsSwordAttack(),null).Invoke();
        private void AimSpecialAttack(InputAction.CallbackContext obj)
        {
            (pointer,spell) = SpellCreator.LaunchSpell(SpellCreator.SpellTypes.IceSpell);
            
            AnimationAction(AnimationNames.IsSpecialAttack(), null).Invoke();
            
            _speed = 0;
        }
        private void LaunchSpecialAttack(InputAction.CallbackContext obj)
        {
            ResumeAnimatior();
            _speed = 2;
        }
        
        private Action AnimationAction (string animationName,object type)
        {
            return type switch
            {
                int   => () => _playerAnimator.SetInteger(animationName,(int)type),
                float => () => _playerAnimator.SetFloat(animationName,(float)type),
                bool  => () => _playerAnimator.SetBool(animationName,(bool)type),
                null  => () => _playerAnimator.SetTrigger(animationName),
                _     =>       throw new ArgumentOutOfRangeException( "animationName: " + animationName + " or type: " + type + " not valid or not implemented")
            };
        }

        private void SetVelocity(Vector2 velocity) => _rigidbody.velocity = velocity * _speed;

        private void SetDirectionWithoutNotify(Vector2 direction) => _direction = direction;

        private void SetDirectionNotifying(Vector2 direction) => Direction = direction;
        
        //EventFunction from the animator
        public void PauseAnimator() => _playerAnimator.speed = 0;
        public void ResumeAnimatior() => _playerAnimator.speed = 1;
    }
}
