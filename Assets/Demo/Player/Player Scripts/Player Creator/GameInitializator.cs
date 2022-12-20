using System;
using Demo.Paths;
using Demo.Player.Player_Scripts.Player_Creator;
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
        }
        
        private void CreatePlayer()
        {
            _playerBuilder = GetBuilder();
            _builderOptions.Builder = _playerBuilder;
            _builderOptions.BuildNormalPlayer();
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