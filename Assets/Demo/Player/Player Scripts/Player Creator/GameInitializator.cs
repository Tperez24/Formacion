using System;
using Demo.GameInputState;
using Demo.Input_Adapter;
using Demo.Paths;
using Demo.Player.Player_Scripts.Player_Creator;
using Demo.Player.Player_Scripts.Player_Installers;
using UnityEngine;

namespace Demo.Player.Player_Scripts.Player_Creator
{
    public class GameInitializator : MonoBehaviour
    {
        public PlayerTypes selectPlayer;
        
        private BuilderOptions _builderOptions;
        
        private IPlayerBuilder _playerBuilder;
        private IInputBuilder _inputBuilder;
        
        private void Start()
        {
            _builderOptions = new BuilderOptions();
            
            CreatePlayer();
            CreateInput();

            SetReferencesIntoInput();
        }
        private void CreateInput()
        {
            _inputBuilder = new InputBuilderConfiguration(Resources.Load<GameObject>(PrefabsPath.Input()));
            _builderOptions.InputBuilder = _inputBuilder;
            _builderOptions.BuildInputSystem();
        }

        private void CreatePlayer()
        {
            _playerBuilder = GetBuilder();
            _builderOptions.Builder = _playerBuilder;
            _builderOptions.BuildNormalPlayer();
        }
        
        private void SetReferencesIntoInput()
        {
            var inputInterface = (PlayerConfigurationInstaller)_playerBuilder.GetPlayerBuilder().parts
                .Find(component => component.GetType() == typeof(PlayerConfigurationInstaller));
            
            var inputController = (InputController)_inputBuilder.GetInputBuilder().parts
                .Find(component => component.GetType() == typeof(InputController));
            
            inputController.SetInput(inputInterface.GetInput());
            inputController.Initialize();
        }
        private IPlayerBuilder GetBuilder()
        {
            return selectPlayer.playerType switch
            {
                PlayerBuilder.PlayerType.MalePlayer => new MalePlayer(selectPlayer.playerPrefab),
                PlayerBuilder.PlayerType.FemalePlayer => new FemalePlayer(selectPlayer.playerPrefab),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}

[Serializable]
public struct PlayerTypes
{
    public PlayerBuilder.PlayerType playerType;
    public GameObject playerPrefab;
}