using System;
using Demo.Input_Adapter;
using Demo.Player.PlayerMediator;

using Demo.Scripts.StaticClasses;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Demo.Player.Player_Scripts.Player_Behaviour
{
    public class PlayerController : PlayerComponents
    {
        private IInput Input { get; set; }
        private Animator _playerAnimator;
        private InputAction _moveAction,_attackAction,_specialAction;
        private Vector2 _direction,_lastDirection;
        private object _lastValueGiven;
        private Rigidbody2D _rigidbody;

        private float _speed = 2f;
        private UnityEvent _onDirectionChanged = new UnityEvent();
        public UnityEvent<Vector2> onMoveInputChanged = new UnityEvent<Vector2>();
        
        public void SetInput(IInput input) => Input = input;

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
            SetAnimatorToAdapter();
            SetActions();
            InitializeEvents();
            SubscribeToInputs();
            SubscribeToEvents();
            SetSpeed(2);
        }
        
        private void OnDisable()
        {
            _moveAction.performed -= ChangeDirection;
            _attackAction.performed -= Attack;
            _specialAction.performed -= LaunchSpecialAttack;
            _specialAction.started -= AimSpecialAttack;
            _specialAction.canceled -= SpecialAttackCanceled;
        }

        private void Getters()
        {
            _playerAnimator = GetComponentInParent<Animator>();
            _rigidbody = GetComponentInParent<Rigidbody2D>();
        }

        private void SetAnimatorToAdapter() => Mediator.Notify(this,"SetAnimator");

        private void SetActions()
        {
            _moveAction = Input.GetInput().Find(input => input.name == ActionNames.Movement());
            _attackAction = Input.GetInput().Find(input => input.name == ActionNames.Attack());
            _specialAction = Input.GetInput().Find(input => input.name == ActionNames.Special());
        }

        private void InitializeEvents()
        {
            onMoveInputChanged = new UnityEvent<Vector2>();
            _onDirectionChanged = new UnityEvent();
        }

        private void SubscribeToInputs()
        {
            _moveAction.performed += ChangeDirection;
            _attackAction.performed += Attack;
            _specialAction.performed += LaunchSpecialAttack;
            _specialAction.started += AimSpecialAttack;
            _specialAction.canceled += SpecialAttackCanceled;
        }

        private void SubscribeToEvents()
        {
            _onDirectionChanged = new UnityEvent();
            _onDirectionChanged.AddListener(MovePlayer);
        }

        private void ChangeDirection(InputAction.CallbackContext context)
        {
            var dir = context.ReadValue<Vector2>();
            onMoveInputChanged?.Invoke(dir);
           
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
        
        private void Attack(InputAction.CallbackContext context)
        {
            Mediator.Notify(this,"LaunchAttack");
            
            AnimationAction(AnimationNames.IsSwordAttack(), null).Invoke();
        }

        private void AimSpecialAttack(InputAction.CallbackContext obj)
        {
            SetSpeed(0);
            Mediator.Notify(this,"AimSpecialAttack");
        }

        private void LaunchSpecialAttack(InputAction.CallbackContext obj)
        {
            SetSpeed(2);
            Mediator.Notify(this,"LaunchSpecialAttack");
        }

        private void SpecialAttackCanceled(InputAction.CallbackContext obj)
        {
            SetSpeed(2);
            Mediator.Notify(this,"SpecialAttackCanceled");
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

        private void SetSpeed(float speed) => _speed = speed;

        private void SetDirectionWithoutNotify(Vector2 direction) => _direction = direction;

        private void SetDirectionNotifying(Vector2 direction) => Direction = direction;
    }
}

