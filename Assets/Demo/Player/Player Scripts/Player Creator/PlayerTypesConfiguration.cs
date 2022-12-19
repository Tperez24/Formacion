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

        public void AddPlayerAttackController()
        {
            var attackControllerGo = new GameObject("Attack Controller");
            attackControllerGo.transform.SetParent(_player.transform);

            _attackAdapter = attackControllerGo.AddComponent<AttackAdapter>();
            _attackAdapter.SetPlayerController(_behaviour);

            _player.Add(_attackAdapter);
        }

        public void AddPlayerAbilityTree()
        {
            var abilityTreeGo = new GameObject("Player Ability Tree");
            abilityTreeGo.transform.SetParent(_player.transform);

            _playerWeaponsComposite = abilityTreeGo.AddComponent<PlayerWeaponsComposite>();
            _attackAdapter.SetAbilityTree(_playerWeaponsComposite,AttackAdapter.AttackType.Spell);
            
            _player.Add(_playerWeaponsComposite);
        }

        public void AddPlayerMediator()
        {
            var playerMediatorGo = new GameObject("Player Mediator");
            playerMediatorGo.transform.SetParent(_player.transform);

            _playerMediator = playerMediatorGo.AddComponent<PlayerMediator.PlayerMediator>();
            _playerMediator.SetReferences(_behaviour,_installer,_attackAdapter,_playerAnimator);
            
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
            SetComponentOnMediator(_behaviour);
            
            _installer.Initialize();
            _behaviour.Initialize();
        }

        private void SetComponentOnMediator(PlayerComponents component) => component.SetMediator(_playerMediator);
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

        public void AddPlayerAttackController() => throw new System.NotImplementedException();

        public void AddPlayerBehaviour() => throw new System.NotImplementedException();

        public PlayerBuilder GetPlayer() => throw new System.NotImplementedException();

        public void Initialize() => throw new System.NotImplementedException();

        public void AddPlayerAbilityTree() => throw new System.NotImplementedException();
    }
}