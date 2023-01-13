using Demo.GameInputState;
using Demo.Player.Player_Scripts.Player_Installers;
using UnityEngine;

namespace Demo.Player.Player_Scripts.Player_Creator
{
    public class InputBuilderConfiguration : IInputBuilder
    {
        private readonly InputBuilder _inputBuilder;
        private InputController _inputController;
        private InputConfigurationInstaller _inputConfiguration;
        public InputBuilderConfiguration(GameObject inputPrefab)
        {
            var input = Object.Instantiate(inputPrefab);
            _inputBuilder = input.AddComponent<InputBuilder>();
        }
        public void AddInputController()
        {
            var inputControllerGo = new GameObject("InputController");
            inputControllerGo.transform.SetParent(_inputBuilder.transform);
            
            _inputController = inputControllerGo.AddComponent<InputController>();
            _inputBuilder.Add(_inputController);
        }
        
        public void AddInputConfigurationInstaller()
        {
            var inputConfiguration = new GameObject("Input Configuration");
            inputConfiguration.transform.SetParent(_inputBuilder.transform);
            
            _inputConfiguration = inputConfiguration.AddComponent<InputConfigurationInstaller>();

            _inputController.SetInput(_inputConfiguration.GetInput());
            _inputController.Initialize();
            
            _inputBuilder.Add(_inputConfiguration);
        }

        public InputBuilder GetInputBuilder() => _inputBuilder;
    }
}