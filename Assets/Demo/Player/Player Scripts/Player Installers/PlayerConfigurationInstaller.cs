using System.Linq;
using Demo.Input_Adapter;
using Demo.Player.Player_Scripts.Player_Behaviour;
using Demo.Player.Spells.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Demo.Player.Player_Scripts.Player_Installers
{
    public class PlayerConfigurationInstaller : MonoBehaviour
    {
        private PlayerController _playerBehaviour;
        private AttackAdapter _attackAdapter;

        public void SetPlayerController(PlayerController behaviour) => _playerBehaviour = behaviour;
        public void SetPlayerAttackAdapter(AttackAdapter attackAdapter) => _attackAdapter = attackAdapter;

        public void Initialize()
        {
            _playerBehaviour.SetInput(GetInput());
            _playerBehaviour.SetAdapter(_attackAdapter);
        }

        private IInput GetInput()
        {
            var devices = InputSystem.devices;
            
            return devices.Any(device => device.name == "XInputControllerWindows")
                ? new XboxInputAdapter()
                : new KeyboardInputAdapter();
        }
    }
}