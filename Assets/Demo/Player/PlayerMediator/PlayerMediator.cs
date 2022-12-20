using System;
using Demo.Player.Player_Scripts.Player_Behaviour;
using Demo.Player.Spells.Scripts;
using Demo.ProjectileComposite;
using Demo.Scripts.StaticClasses;
using UnityEngine;
using UnityEngine.Events;

namespace Demo.Player.PlayerMediator
{
    public class PlayerMediator : MonoBehaviour,IPlayerComponentsMediator
    {
        private PlayerController _playerController;
        private Animator _playerAnimator;

        private AttackAdapter _attackAdapter;
        private AttackController _attackController;
        private PlayerWeaponsComposite _weaponsComposite;
        private SpellAttackController _spellAttackController;
        
        public void SetReferences(PlayerController playerController, AttackAdapter attackAdapter, Animator playerAnimator,
            AttackController attackController,SpellAttackController spellAttackController,PlayerWeaponsComposite weaponsComposite)
        {
            _playerController = playerController;
            _attackAdapter = attackAdapter;
            _playerAnimator = playerAnimator;
            _attackController = attackController;
            _spellAttackController = spellAttackController;
            _weaponsComposite = weaponsComposite;
        }
        public void Notify(object sender, string methodName)
        {
            if (sender.GetType() == typeof(PlayerController)) SearchOnPlayerDependencies(methodName);
            if (sender.GetType() == typeof(AttackController)) SearchOnAttackControllerDependencies(methodName);
            if (sender.GetType() == typeof(SpellAttackController)) SearchOnSpellAttackControllerDependencies(methodName);
        }
        
        public void SubscribeTo(string subscribeFrom, UnityAction<Vector2> ev,bool subscribe)
        {
            if(subscribeFrom == "PlayerMoveSubscription" && subscribe) _playerController.onMoveInputChanged.AddListener(ev);
            if(subscribeFrom == "PlayerMoveSubscription" && !subscribe) _playerController.onMoveInputChanged.RemoveListener(ev);
        }

        public object GetReference(string dependency) => GetDependenciesOf(dependency);

        private void SearchOnPlayerDependencies(string methodName)
        {
            Action action = methodName switch
            {
                "LaunchAttack" => () => _attackAdapter.LaunchAttack(AttackAdapter.AttackType.Normal),
                "LaunchSpecialAttack" => () => _attackAdapter.LaunchAttack(AttackAdapter.AttackType.Spell),
                "AimSpecialAttack" => () => _attackAdapter.StartCharging(AttackAdapter.AttackType.Spell),
                "SpecialAttackCanceled" => () => _attackAdapter.AttackCanceled(AttackAdapter.AttackType.Spell),
            };
            action.Invoke();
        }
        private void SearchOnSpellAttackControllerDependencies(string methodName)
        {
            Action action = methodName switch
            {
                "LaunchAttack" => () => _attackAdapter.LaunchAttack(AttackAdapter.AttackType.Normal),
                "LaunchSpecialAttack" => () =>
                {
                    _attackAdapter.LaunchAttack(AttackAdapter.AttackType.Spell);
                    _playerAnimator.speed = 2;
                },
                "TriggerSpecialAttack" => () => _playerAnimator.SetTrigger(AnimationNames.IsSpecialAttack()),
                "PausePlayerAnimator" => () => _playerAnimator.speed = 0,
                "ResumePlayerAnimator" => () => _playerAnimator.speed = 2,
                _ => throw new ArgumentOutOfRangeException(nameof(methodName), methodName, null)
            };
            
            action.Invoke();
        }

        private void SearchOnAttackControllerDependencies(string methodName)
        {
            Action action = methodName switch
            {
                "TriggerNormalAttack" => () => _playerAnimator.SetTrigger(AnimationNames.IsSwordAttack()),
                _ => throw new ArgumentOutOfRangeException(nameof(methodName), methodName, null)
            };
            
            action.Invoke();
        }

        private object GetDependenciesOf(string dependency)
        {
            return dependency switch
            {
                "AttackController" => _attackController,
                "SpellController" => _spellAttackController,
                "PlayerPosition" => _playerController.transform.position,
                "CompositeAnimator" => _weaponsComposite.GetAoc(AttackAdapter.AttackType.Spell),
                "CompositeType" => _weaponsComposite.GetSpellType(),
                "GetPlayerAnimator" => _playerAnimator,
                _ => throw new ArgumentOutOfRangeException(nameof(dependency), dependency, null)
            };
        }
    }
}