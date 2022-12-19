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
       
        public void SetPlayerController(PlayerController behaviour) => _playerBehaviour = behaviour;
        public void Initialize() => _playerBehaviour.SetInput(GetInput());
        private IInput GetInput()
        {
            var devices = InputSystem.devices;
            
            return devices.Any(device => device.name == "XInputControllerWindows")
                ? new XboxInputAdapter()
                : new KeyboardInputAdapter();
        }
    }
}