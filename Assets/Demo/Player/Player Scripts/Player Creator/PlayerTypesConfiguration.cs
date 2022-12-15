using Player.Player_Scripts;
using UnityEngine;

namespace Demo.Player.Player_Scripts.Player_Creator
{
    public class NormalPlayer : IPlayerBuilder
    {
        private readonly PlayerBuilder _player;
        private PlayerController _behaviour;
        private PlayerConfigurationInstaller _installer;
        
        public NormalPlayer(GameObject playerPrefab)
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
            _installer.Initialize();
            _behaviour.Initialize();
        }

        public PlayerBuilder GetPlayer() => _player;
    }
}