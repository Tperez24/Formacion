using Demo.Player.Player_Scripts.Player_Behaviour;
using Demo.Player.Player_Scripts.Player_Installers;
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
        }

        public void AddPlayerConfiguration()
        {
            var playerConfig = new GameObject("Player Configuration");
            playerConfig.transform.SetParent(_player.transform);
            
            _installer = playerConfig.AddComponent<PlayerConfigurationInstaller>();
            _installer.SetPlayerController(_behaviour);
            _installer.SetPlayerAttackAdapter(_attackAdapter);

            _player.Add(_installer);
        }

        public void Initialize()
        {
            _installer.Initialize();
            _behaviour.Initialize();
        }

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
        public void AddPlayerConfiguration()
        {
            throw new System.NotImplementedException();
        }

        public void AddPlayerAttackController()
        {
            throw new System.NotImplementedException();
        }

        public void AddPlayerBehaviour()
        {
            throw new System.NotImplementedException();
        }

        public PlayerBuilder GetPlayer()
        {
            throw new System.NotImplementedException();
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void AddPlayerAbilityTree()
        {
            throw new System.NotImplementedException();
        }
    }
}