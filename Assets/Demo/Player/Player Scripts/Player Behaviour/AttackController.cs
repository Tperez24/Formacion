using System;
using System.Collections;
using Demo.AnimatorChecker;
using Demo.Player.Spells;
using Demo.Player.Spells.Scripts;
using Demo.Projectile_Abstract_Factory;
using Demo.ProjectileComposite;
using Demo.Scripts.StaticClasses;
using UnityEngine;

namespace Demo.Player.Player_Scripts.Player_Behaviour
{ 
    public class SpellAttackController : MonoBehaviour,IAttack
    {
        private PlayerController _playerController;
        private PlayerWeaponsComposite _playerWeaponsComposite;
        private Animator _playerAnimator;
        
        private Coroutine _waitAnimationEnd,_movePointer;

        private IAbstractPointer _pointer;
        private IAbstractSpell _spell;

        public void SetPlayerController(PlayerController playerController) => _playerController = playerController;
        public void SetAnimator(Animator animator) => _playerAnimator = animator;
        private void SubscribeToEvents() => _playerController.OnMoveInputChanged.AddListener(MovePointer);
        private void UnsubscribeToEvents() => _playerController.OnMoveInputChanged.RemoveListener(MovePointer);
        
        public void Charge()
        {
            (_pointer,_spell) = SpellCreator.LaunchSpell(_playerWeaponsComposite.GetSpellType());

            InitializeSpell();
            InitializePointer();
            SubscribeToEvents();
            StartCoroutine(WaitForAnimationEnds());
        }

        private void InitializePointer() => _pointer.Translate(_playerController.gameObject.transform.position);

        private void InitializeSpell()
        {
            _spell.SetAoc(_playerWeaponsComposite.GetAoc(AttackAdapter.AttackType.Spell));
            _spell.Enable(false);
        }

        public void Launch()
        {
            ResumeAnimator();
            
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
            ResumeAnimator();
            UnsubscribeToEvents();
            
            StopCoroutine(_waitAnimationEnd);
            StopCoroutine(_movePointer);

            DestroyPointer();
            DestroySpell();
        }
        private void DestroySpell() => _spell.DestroyParent();
        public void SetWeaponsComposite(PlayerWeaponsComposite playerWeaponsComposite) => _playerWeaponsComposite = playerWeaponsComposite;
        private void MovePointer(Vector2 direction)
        {
            if(_movePointer != null) StopCoroutine(_movePointer);
            _movePointer = StartCoroutine(InputMovementPressed(direction, () => _pointer.MovePointer(direction,4)));
        }

        private IEnumerator WaitForAnimationEnds()
        {
            _playerAnimator.SetTrigger(AnimationNames.IsSpecialAttack());

            yield return null;
            
            _waitAnimationEnd = StartCoroutine(AnimationChecks.CheckForAnimationChanged(_playerAnimator, PauseAnimator));
        }
        private void PauseAnimator() => _playerAnimator.speed = 0;
        private void ResumeAnimator() => _playerAnimator.speed = 1;
        
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
    public class AttackController : MonoBehaviour,IAttack
    {
        private Animator _playerAnimator;
        public void SetAnimator(Animator animator) => _playerAnimator = animator;
        public void Charge() => throw new NotImplementedException();
        public void Launch() => _playerAnimator.SetTrigger(AnimationNames.IsSwordAttack());
        public void Cancel() => throw new NotImplementedException();
        public void SetWeaponsComposite(PlayerWeaponsComposite playerWeaponsComposite) => throw new NotImplementedException();
    }
}