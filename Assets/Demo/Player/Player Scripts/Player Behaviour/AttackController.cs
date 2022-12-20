using System;
using System.Collections;
using Demo.AnimatorChecker;
using Demo.GameInputState;
using Demo.Player.PlayerMediator;
using Demo.Player.Spells.Scripts;
using Demo.Projectile_Abstract_Factory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Demo.Player.Player_Scripts.Player_Behaviour
{ 
    public class SpellAttackController : PlayerComponent,IAttack
    {
        private Coroutine _waitAnimationEnd,_movePointer;

        private IAbstractPointer _pointer;
        private IAbstractSpell _spell;

        protected SpellAttackController(IPlayerComponentsMediator mediator) : base(mediator) { }

        private void OnEnable()
        {
            PlayerInputState.ChargeSpecial += Charge;
           
            PointerInputState.MovePointer += MovePointer;
            PointerInputState.CancelSpecial += Cancel;
            PointerInputState.LaunchSpecial += Launch;
        }

        private void OnDisable()
        {
            PlayerInputState.ChargeSpecial -= Charge;
         
            PointerInputState.MovePointer -= MovePointer;
            PointerInputState.CancelSpecial -= Cancel;
            PointerInputState.LaunchSpecial -= Launch;
        }

        public void Charge(object sender, InputAction.CallbackContext callbackContext)
        {
            (_pointer,_spell) = SpellCreator.LaunchSpell((SpellCreator.SpellTypes)Mediator.GetReference(MediatorActionNames.CompositeType()));
            
            InitializeSpell();
            InitializePointer();
            StartCoroutine(WaitForAnimationEnds());
        }

        public void Launch(object sender, InputAction.CallbackContext callbackContext)
        {
            Mediator.Notify(this,MediatorActionNames.ResumePlayerAnimator());
            
            StopCoroutine(_movePointer);

            SetSpellToPointerPos();
            DestroyPointer();
            
            StartCoroutine(AnimationChecks.CheckForAnimationFinished(_spell.GetAnimator(), _spell.DestroyParent));
        }

        public void Cancel(object sender, InputAction.CallbackContext callbackContext)
        {
            Mediator.Notify(this,MediatorActionNames.ResumePlayerAnimator());

            StopCoroutine(_waitAnimationEnd);
            StopCoroutine(_movePointer);

            DestroyPointer();
            DestroySpell();
        }

        private void InitializeSpell()
        {
            _spell.SetAoc(Mediator.GetReference(MediatorActionNames.CompositeAnimator()) as AnimatorOverrideController);
            _spell.Enable(false);
        }

        private void InitializePointer() => _pointer.Translate((Vector3)Mediator.GetReference(MediatorActionNames.PlayerPosition()));

        private void SetSpellToPointerPos()
        {
            _spell.Translate(_pointer);
            _spell.Enable(true);
        }

        private void DestroyPointer() => _pointer.Destroy();
        private void DestroySpell() => _spell.DestroyParent();
        private void MovePointer(object sender, InputAction.CallbackContext callbackContext)
        {
            var direction = callbackContext.ReadValue<Vector2>();
            if(_movePointer != null) StopCoroutine(_movePointer);
            _movePointer = StartCoroutine(InputMovementPressed(direction, () => _pointer.MovePointer(direction,4)));
        }

        private IEnumerator WaitForAnimationEnds()
        {
            Mediator.Notify(this,MediatorActionNames.TriggerSpecialAttack());

            yield return null;
            
            _waitAnimationEnd = StartCoroutine(AnimationChecks.CheckForAnimationChanged((Animator)Mediator.GetReference
                (MediatorActionNames.GetPlayerAnimator()), () => Mediator.Notify(this,MediatorActionNames.PausePlayerAnimator())));
        }

        //TODO Observer¿?
        private IEnumerator InputMovementPressed(Vector2 direction,Action action)
        {
            do
            {
                action?.Invoke();
                yield return null;
                
            } while (direction != Vector2.zero);
        }
    }
    public class AttackController : PlayerComponent,IAttack
    {
        protected AttackController(IPlayerComponentsMediator mediator) : base(mediator) { }
        public void Charge(object sender, InputAction.CallbackContext callbackContext) => throw new NotImplementedException();
        public void Launch(object sender, InputAction.CallbackContext callbackContext) => throw new NotImplementedException();
        public void Cancel(object sender, InputAction.CallbackContext callbackContext) => throw new NotImplementedException();
    }
}