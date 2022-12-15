using System;
using System.Collections;
using Demo.AnimatorChecker;
using Demo.Player.Player_Scripts.Player_Behaviour;
using Demo.Projectile_Abstract_Factory;
using Demo.Scripts.StaticClasses;
using Unity.VisualScripting;
using UnityEngine;

namespace Demo.Player.Player_Scripts.Player_Behaviour
{
    public class AttackAdapter : MonoBehaviour
    {
        public enum AttackType
        {
            Spell,
            Normal
        }
        
        private IAttack GetAttack(AttackType attackType)
        {
            return attackType switch
            {
                AttackType.Normal => TryGetComponent(out AttackController attack)
                    ? attack
                    : gameObject.AddComponent<AttackController>(),
                AttackType.Spell => TryGetComponent(out SpellAttackController spell)
                    ? spell
                    : gameObject.AddComponent<SpellAttackController>(),
                _ => null
            };
        }
        public void StartCharging(AttackType type) => GetAttack(type).Charge();
        public void LaunchAttack(AttackType type) => GetAttack(type).Launch();
        public void AttackCanceled(AttackType type) => GetAttack(type).Cancel();
        public void SetAnimator(Animator animator,AttackType type) => GetAttack(type).SetAnimator(animator);
    }
    public class SpellAttackController : MonoBehaviour,IAttack
    {
        private Animator _playerAnimator;
        IAbstractPointer _pointer;
        IAbstractSpell _spell;

        public void SetAnimator(Animator animator) => _playerAnimator = animator;
        
        public void Charge()
        {
            (_pointer,_spell) = SpellCreator.LaunchSpell(SpellCreator.SpellTypes.IceSpell);
            
            StartCoroutine(WaitForAnimationEnds());
        }

        public void Launch()
        {
            ResumeAnimator();
        }

        public void Cancel()
        {
            ResumeAnimator();
        }
        private IEnumerator WaitForAnimationEnds()
        {
            _playerAnimator.SetTrigger(AnimationNames.IsSpecialAttack());

            yield return null;
            
            StartCoroutine(AnimationChecks.CheckForAnimationFinished(_playerAnimator, PauseAnimator));
        }
        
        private void PauseAnimator() => _playerAnimator.speed = 0;

        private void ResumeAnimator() => _playerAnimator.speed = 1;
    }
    public class AttackController : MonoBehaviour,IAttack
    {
        private Animator _playerAnimator;
        public void SetAnimator(Animator animator) => _playerAnimator = animator;
        
        public void Charge()
        {
            throw new NotImplementedException();
        }

        public void Launch()
        {
            _playerAnimator.SetTrigger(AnimationNames.IsSwordAttack());
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }
    }

    
}
public interface IAttack
{
    public void SetAnimator(Animator animator);

    public void Charge();
    public void Launch();
    public void Cancel();
}