using System.Linq;
using Demo.Input_Adapter;
using Player.Player_Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Demo.Player.Player_Scripts
{
    public class PlayerConfigurationInstaller : MonoBehaviour
    {
        //TODO Mas tarde el player no estará referenciado si no que se creará en tiempo real, entonces tendrá un constructor que añadirá el input directamente
        [SerializeField] private PlayerController player;

        public void Awake()
        {
            player.SetInput(GetInput());
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