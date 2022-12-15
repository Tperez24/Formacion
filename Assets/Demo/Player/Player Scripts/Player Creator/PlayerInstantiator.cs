using System;
using UnityEngine;

namespace Demo.Player.Player_Scripts.Player_Creator
{
    public class PlayerInstantiator : MonoBehaviour
    {
        public PlayerBuilder.PlayerType playerType;
        public GameObject normalPlayerPrefab;

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
            return playerType switch
            {
                PlayerBuilder.PlayerType.NormalPlayer => new NormalPlayer(normalPlayerPrefab),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}