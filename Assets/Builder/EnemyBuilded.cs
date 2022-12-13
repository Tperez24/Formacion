using System.Collections.Generic;
using UnityEngine;

namespace Builder
{
    public class EnemyBuilded : MonoBehaviour
    {
        public List<GameObject> parts = new List<GameObject>();

        public void Add(GameObject part) => parts.Add(part);
    }
}
