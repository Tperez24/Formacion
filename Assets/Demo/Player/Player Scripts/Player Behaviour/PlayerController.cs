using System;
using Demo.GameInputState;
using Demo.Player.PlayerMediator;
using Demo.Scripts.StaticClasses;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Demo.Player.Player_Scripts.Player_Behaviour
{
    public class PlayerController : PlayerComponent
    {
        private Animator _playerAnimator;
        private Vector2 _direction,_lastDirection;
        private object _lastValueGiven;
        private Rigidbody2D _rigidbody;

        private float _speed = 3f;
        private UnityEvent _onDirectionChanged = new();
        protected PlayerController(IPlayerComponentsMediator mediator) : base(mediator) { }
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
            SubscribeToEvents();
            SetSpeed(3);
        }

        private void Getters()
        {
            _playerAnimator = GetComponentInParent<Animator>();
            _rigidbody = GetComponentInParent<Rigidbody2D>();
        }

        private void SubscribeToEvents()
        {
            _onDirectionChanged = new UnityEvent();
            _onDirectionChanged.AddListener(MovePlayer);

            PlayerInputState.MovePlayer += ChangeDirection;
            PlayerInputState.ChargeSpecial += StopRigidbody;
            PlayerInputState.Attack += Attack;
            
            PointerInputState.CancelSpecial += StopPlayerMovement;
            PointerInputState.LaunchSpecial += StopPlayerMovement;
        }

        private void OnDisable()
        {
            _onDirectionChanged.RemoveListener(MovePlayer);

            PlayerInputState.MovePlayer -= ChangeDirection;
            PlayerInputState.ChargeSpecial -= StopRigidbody;
            PlayerInputState.Attack -= Attack;

            PointerInputState.CancelSpecial -= StopPlayerMovement;
            PointerInputState.LaunchSpecial -= StopPlayerMovement;
        }

        private void ChangeDirection(object sender, InputAction.CallbackContext context)
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
        
        private void Attack(object sender, InputAction.CallbackContext callbackContext) => AnimationAction(AnimationNames.IsSwordAttack(), null).Invoke();

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

        private void SetSpeed(float speed) => _speed = speed;

        private void SetDirectionWithoutNotify(Vector2 direction) => _direction = direction;

        private void SetDirectionNotifying(Vector2 direction) => Direction = direction;
        
        private void StopRigidbody(object sender, InputAction.CallbackContext e) => SetVelocity(Vector2.zero);

        private void StopPlayerMovement(object sender, InputAction.CallbackContext e) => StopPlayer();
    }
}

