using System;
using System.Collections;
using Demo.AnimatorChecker;
using Demo.Player.PlayerMediator;
using Demo.Player.Spells.Scripts;
using Demo.Projectile_Abstract_Factory;
using UnityEngine;

namespace Demo.Player.Player_Scripts.Player_Behaviour
{ 
    public class SpellAttackController : PlayerComponent,IAttack
    {
        private Coroutine _waitAnimationEnd,_movePointer;

        private IAbstractPointer _pointer;
        private IAbstractSpell _spell;

        protected SpellAttackController(IPlayerComponentsMediator mediator) : base(mediator) { }
        private void SubscribeToEvents() => Mediator.SubscribeTo(MediatorActionNames.PlayerMoveSubscription(), MovePointer,true);
        private void UnsubscribeToEvents() => Mediator.SubscribeTo(MediatorActionNames.PlayerMoveSubscription(), MovePointer,false);
        
        public void Charge()
        {
            (_pointer,_spell) = SpellCreator.LaunchSpell((SpellCreator.SpellTypes)Mediator.GetReference(MediatorActionNames.CompositeType()));

            SubscribeToEvents();
            InitializeSpell();
            InitializePointer();
            StartCoroutine(WaitForAnimationEnds());
        }

        private void InitializePointer() => _pointer.Translate((Vector3)Mediator.GetReference(MediatorActionNames.PlayerPosition()));

        private void InitializeSpell()
        {
            _spell.SetAoc(Mediator.GetReference(MediatorActionNames.CompositeAnimator()) as AnimatorOverrideController);
            _spell.Enable(false);
        }

        public void Launch()
        {
            UnsubscribeToEvents();
            StopCoroutine(_movePointer);

            SetSpellToPointerPos();
            DestroyPointer();
            
            StartCoroutine(AnimationChecks.CheckForAnimationFinished(_spell.GetAnimator(), _spell.DestroyParent));
        }

        private void SetSpellToPointerPos()
        {
            _spell.Translate(_pointer);
            _spell.Enable(true);
        }
        private void DestroyPointer() => _pointer.Destroy();

        public void Cancel()
        {
            Mediator.Notify(this,MediatorActionNames.ResumePlayerAnimator());
            UnsubscribeToEvents();
            
            StopCoroutine(_waitAnimationEnd);
            StopCoroutine(_movePointer);

            DestroyPointer();
            DestroySpell();
        }
        private void DestroySpell() => _spell.DestroyParent();
        private void MovePointer(Vector2 direction)
        {
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
        public void Charge() => throw new NotImplementedException();
        public void Launch() => Mediator.Notify(this,MediatorActionNames.TriggerNormalAttack());
        public void Cancel() => throw new NotImplementedException();
    }
}