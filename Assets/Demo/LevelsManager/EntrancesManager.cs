using System.Collections.Generic;
using Demo.LevelsManager.ChangeRoom;
using UnityEngine;

namespace Demo.LevelsManager
{
    public class EntrancesManager : MonoBehaviour
    {
        public static EntrancesManager Instance { get; set; }
        private List<CreateNewRoom> _entrances;
        private void Awake()
        {
            _entrances = new List<CreateNewRoom>();
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public void AddEntrance(CreateNewRoom entrance) => _entrances.Add(entrance);

        public Vector3 ChainEntrances(Vector3 entranceToChain, EntranceType.EntrancesTypes enter, ScriptableLevel actualLevel, int levelToLoad)
        {
            var connectedEntrance = _entrances.Find(entrance =>
                entrance.GetConnectedEntrance() == Vector3.zero && entrance.GetEntrance().exit == enter && entrance.GetActualLevel() != actualLevel
                && levelToLoad == entrance.GetActualLevel().levelIndex);

            var position = connectedEntrance.transform.position;
            connectedEntrance.SetConnectedEntrance(entranceToChain);

            return position;
        }
    }
}