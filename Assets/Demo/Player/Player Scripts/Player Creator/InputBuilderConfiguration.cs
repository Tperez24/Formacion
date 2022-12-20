using Demo.GameInputState;
using Unity.VisualScripting;
using UnityEngine;

namespace Demo.Player.Player_Scripts.Player_Creator
{
    public class InputBuilderConfiguration : IInputBuilder
    {
        private InputBuilder _inputBuilder;
        private InputController _inputController;
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
    }
}