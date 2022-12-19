using System;
using Demo.Player.Player_Scripts.Player_Behaviour;
using Demo.Player.Player_Scripts.Player_Installers;
using Demo.Player.Spells.Scripts;
using Demo.ProjectileComposite;
using UnityEngine;

namespace Demo.Player.PlayerMediator
{
    public class PlayerMediator : MonoBehaviour,IPlayerComponentsMediator
    {
        private PlayerController _playerController;
        private PlayerConfigurationInstaller _playerConfigurationInstaller;
        private Animator _playerAnimator;

        private AttackAdapter _attackAdapter;
        private AttackController _attackController;
        private PlayerWeaponsComposite _weaponsComposite;
        private SpellAttackController _spellAttackController;
        
        public void SetReferences(PlayerController playerController, PlayerConfigurationInstaller playerConfigurationInstaller, AttackAdapter attackAdapter, Animator playerAnimator)
        {
            _playerController = playerController;
            _playerConfigurationInstaller = playerConfigurationInstaller;
            _attackAdapter = attackAdapter;
            _playerAnimator = playerAnimator;
        }
        public void Notify(object sender, string methodName)
        {
            if (sender.GetType() == typeof(PlayerController)) SearchOnPlayerDependencies(methodName);
            if (sender.GetType() == typeof(AttackController)) SearchOnAttackControllerDependencies(methodName);
        }

        private void SearchOnPlayerDependencies(string methodName)
        {
            switch (methodName)
            {
                case "SetAnimator":
                    _attackAdapter.SetAnimator(_playerAnimator, AttackAdapter.AttackType.Normal);
                    _attackAdapter.SetAnimator(_playerAnimator, AttackAdapter.AttackType.Spell);
                    break;
                case "LaunchAttack":
                    _attackAdapter.LaunchAttack(AttackAdapter.AttackType.Normal);
                    break;
                case "LaunchSpecialAttack":
                    _attackAdapter.LaunchAttack(AttackAdapter.AttackType.Spell); 
                    break;
                case "AimSpecialAttack":
                    _attackAdapter.StartCharging(AttackAdapter.AttackType.Spell);
                    break;
                case "SpecialAttackCanceled":
                    _attackAdapter.AttackCanceled(AttackAdapter.AttackType.Spell);
                    break;
            }
        }

        private void SearchOnAttackControllerDependencies(string methodName)
        {
            switch (methodName)
            {
                case "SetAnimator":
                    _attackController.SetAnimator(_playerAnimator);
                    break;
                case "LaunchAttack":
                    _attackAdapter.LaunchAttack(AttackAdapter.AttackType.Normal);
                    break;
                case "LaunchSpecialAttack":
                    _attackAdapter.LaunchAttack(AttackAdapter.AttackType.Spell); 
                    break;
            }
        }
    }
}