using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Player.Player_Scripts.Player_Creator
{
    public class PlayerBuilder : MonoBehaviour
    {
        public List<Component> parts = new List<Component>();

        public void Add(Component part) => parts.Add(part);
        public enum PlayerType
        {
            NormalPlayer,
        }
    }
}