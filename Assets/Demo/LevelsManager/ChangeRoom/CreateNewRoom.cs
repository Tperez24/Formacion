using System.Collections.Generic;
using Demo.Player.Player_Scripts.Player_Behaviour;
using Demo.Player.Player_Scripts.Player_Creator;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Demo.LevelsManager.ChangeRoom
{
    public class CreateNewRoom : MonoBehaviour
    {
        private List<ScriptableLevel> _levelsDb;
        private Vector3Int _position;
        public SavedTile entrance,connectedEntrance;
        public TileMapManager tileMapManager;
        public ScriptableLevel levelToLoad,actualLevel;

        private void Start()
        {
            tileMapManager = FindObjectOfType<TileMapManager>();
            actualLevel = tileMapManager.GetActualLevel(_levelsDb,_position);
            entrance = tileMapManager.GetTileAtPosition(_position,actualLevel);
        }

        public void SetLevels(List<ScriptableLevel> levelsDb) => _levelsDb = levelsDb; 
        public void SetTilePosition(Vector3Int position) => _position = position;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.TryGetComponent<PlayerBuilder>(out _)) return;
            tileMapManager.AddLevelOffset(10);
            (levelToLoad,connectedEntrance) = tileMapManager.FindLevelsWithEntrances(_levelsDb,entrance.exit);
            tileMapManager.LoadMap(levelToLoad.levelIndex);
            //FindObjectOfType<PlayerController>().transform.parent.position = tileMapManager.GetTileWorldPosition(connectedEntrance.position);
        }
    }
}
