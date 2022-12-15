using System;
using Demo.Player.Player_Scripts.Player_Creator;
using UnityEngine;

namespace Demo.Player.Player_Scripts.Player_Creator
{
    public class PlayerInstantiator : MonoBehaviour
    {
        public PlayerTypes selectPlayer;
        
        private void Start()
        {
            var director = new BuilderOptions();
            var builder = GetBuilder();
            director.Builder = builder;
         
            director.BuildNormalPlayer();
            
            Debug.Log("Generating player: " + builder);
            foreach (var part in builder.GetPlayer().parts) Debug.Log("Creating part: " + part);
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