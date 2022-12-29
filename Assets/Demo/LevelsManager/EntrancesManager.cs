using System;
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

        public SavedTile ChainEntrances(SavedTile entranceToChain, EntranceType.EntrancesTypes enter, ScriptableLevel actualLevel)
        {
            var connectedEntrance = _entrances.Find(entrance =>
                entrance.GetConnectedEntrance() == null && entrance.GetEntrance().exit == enter && entrance.GetActualLevel() != actualLevel);
            
            connectedEntrance.SetConnectedEntrance(entranceToChain);

            return connectedEntrance.GetEntrance();
        }

        public Vector3 GetEntrancePosition(SavedTile connectedEntrance) => 
            _entrances.Find(entrance => entrance.GetEntrance() == connectedEntrance).transform.position;
    }
}