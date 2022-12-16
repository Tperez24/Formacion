using Demo.Player.Player_Scripts.Player_Behaviour;
using Demo.ProjectileComposite;
using UnityEngine;

namespace Demo.Player.Spells.Scripts
{
    public class AttackAdapter : MonoBehaviour
    {
        private AttackController _attackController;
        private SpellAttackController _spellAttackController;
        private PlayerController _playerController;
        private PlayerWeaponsComposite _playerWeaponsComposite;
        private AttackController AttackController
        {
            get
            {
                if (_attackController != null) return _attackController;
                
                TryGetComponent(out AttackController attack);
                _attackController = attack != null ? attack : gameObject.AddComponent<AttackController>();

                return _attackController;
            }
        }
        private SpellAttackController SpellController 
        {
            get
            {
                if (_spellAttackController != null) return _spellAttackController;
                
                TryGetComponent(out SpellAttackController attack);
                _spellAttackController = attack != null ? attack : gameObject.AddComponent<SpellAttackController>();
                _spellAttackController.SetPlayerController(_playerController);

                return _spellAttackController;
            }
        }

        public void SetPlayerController(PlayerController playerController) => _playerController = playerController;
        
        public enum AttackType
        {
            Spell,
            Normal
        }
        
        private IAttack GetAttack(AttackType attackType)
        {
            return attackType switch
            {
                AttackType.Normal => AttackController,
                AttackType.Spell => SpellController,
                _ => null
            };
        }
        public void StartCharging(AttackType type) => GetAttack(type).Charge();
        public void LaunchAttack(AttackType type) => GetAttack(type).Launch();
        public void AttackCanceled(AttackType type) => GetAttack(type).Cancel();
        public void SetAnimator(Animator animator,AttackType type) => GetAttack(type).SetAnimator(animator);
        public void SetAbilityTree(PlayerWeaponsComposite playerWeaponsComposite,AttackType type) => GetAttack(type).SetWeaponsComposite(playerWeaponsComposite);
    }
}