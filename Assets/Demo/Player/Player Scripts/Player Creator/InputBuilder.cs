using System.Collections.Generic;
using UnityEngine;

namespace Demo.Player.Player_Scripts.Player_Creator
{
    public class InputBuilder: MonoBehaviour
    {
        public List<Component> parts = new List<Component>();

        public void Add(Component part) => parts.Add(part);
    }
}