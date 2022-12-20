using System.Collections.Generic;
using Demo.Player.Player_Scripts.Player_Behaviour;
using Demo.Player.Player_Scripts.Player_Installers;
using Demo.Player.PlayerMediator;
using Demo.Player.Spells.Scripts;
using Demo.ProjectileComposite;
using UnityEngine;

namespace Demo.Player.Player_Scripts.Player_Creator
{
    public class MalePlayer : IPlayerBuilder
    {
        private readonly PlayerBuilder _player;
        private PlayerController _behaviour;
        private AttackAdapter _attackAdapter;
        private AttackController _attackController;
        private SpellAttackController _spellAttackController;
        private PlayerWeaponsComposite _playerWeaponsComposite;
        private PlayerConfigurationInstaller _installer;
        private PlayerMediator.PlayerMediator _playerMediator;
        private Animator _playerAnimator;
        
        public MalePlayer(GameObject playerPrefab)
        {
            var player = Object.Instantiate(playerPrefab);
            _player = player.AddComponent<PlayerBuilder>();
        }

        public void AddPlayerBehaviour()
        {
            var playerBehaviour = new GameObject("Player Behaviour");
            
            playerBehaviour.transform.SetParent(_player.transform);
            _behaviour = playerBehaviour.AddComponent<PlayerController>();

            _playerAnimator = _behaviour.transform.GetComponentInParent<Animator>();
            
            _player.Add(_behaviour);
        }

        public void AddPlayerAttackSystem()
        {
            var attackControllerGo = new GameObject("Attack Controller");
            attackControllerGo.transform.SetParent(_player.transform);

            _attackAdapter = attackControllerGo.AddComponent<AttackAdapter>();
            _spellAttackController = attackControllerGo.AddComponent<SpellAttackController>();
            _attackController = attackControllerGo.AddComponent<AttackController>();

            _player.Add(_attackAdapter);
            _player.Add(_spellAttackController);
            _player.Add(_attackController);
        }

        public void AddPlayerAbilityTree()
        {
            var abilityTreeGo = new GameObject("Player Ability Tree");
            abilityTreeGo.transform.SetParent(_player.transform);

            _playerWeaponsComposite = abilityTreeGo.AddComponent<PlayerWeaponsComposite>();

            _player.Add(_playerWeaponsComposite);
        }

        public void AddPlayerMediator()
        {
            var playerMediatorGo = new GameObject("Player Mediator");
            playerMediatorGo.transform.SetParent(_player.transform);

            _playerMediator = playerMediatorGo.AddComponent<PlayerMediator.PlayerMediator>();

            _player.Add(_playerMediator);
        }

        public void AddPlayerConfiguration()
        {
            var playerConfig = new GameObject("Player Configuration");
            playerConfig.transform.SetParent(_player.transform);
            
            _installer = playerConfig.AddComponent<PlayerConfigurationInstaller>();
            _installer.SetPlayerController(_behaviour);

            _player.Add(_installer);
        }

        public void Initialize()
        {
            SetComponentOnMediator(new List<PlayerComponent>{_behaviour,_attackAdapter,_spellAttackController,_attackController});
            
            SetMediatorReferences();
            
            _installer.Initialize();
            _behaviour.Initialize();
        }

        private void SetComponentOnMediator(List<PlayerComponent> components) =>
            components.ForEach(component => component.SetMediator(_playerMediator));
        private void SetMediatorReferences() => 
            _playerMediator.SetReferences(_behaviour,_attackAdapter,_playerAnimator,_attackController,_spellAttackController,_playerWeaponsComposite);
        public PlayerBuilder GetPlayer() => _player;
    }

    public class FemalePlayer : IPlayerBuilder
    {
        private readonly PlayerBuilder _player;

        public FemalePlayer(GameObject playerPrefab)
        {
            var player = Object.Instantiate(playerPrefab);
            _player = player.AddComponent<PlayerBuilder>();
        }
        public void AddPlayerConfiguration() => throw new System.NotImplementedException();

        public void AddPlayerMediator() => throw new System.NotImplementedException();

        public void AddPlayerAttackSystem() => throw new System.NotImplementedException();

        public void AddPlayerBehaviour() => throw new System.NotImplementedException();

        public PlayerBuilder GetPlayer() => throw new System.NotImplementedException();

        public void Initialize() => throw new System.NotImplementedException();

        public void AddPlayerAbilityTree() => throw new System.NotImplementedException();
    }
}