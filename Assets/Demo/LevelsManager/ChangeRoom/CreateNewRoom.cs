using System.Collections.Generic;
using Demo.Player.Player_Scripts.Player_Creator;
using UnityEngine;
using Random = System.Random;

namespace Demo.LevelsManager.ChangeRoom
{
    public class CreateNewRoom : MonoBehaviour
    {
        private List<ScriptableLevel> _levelsDb;
        private Vector3Int _position;
        public SavedTile tile;
        public TileMapManager tileMapManager;
        public List<ScriptableLevel> levels;
        public int actualLevel;

        private void Start()
        {
            tileMapManager = FindObjectOfType<TileMapManager>();
            tile = tileMapManager.GetTileAtPosition(_position,_levelsDb);
        }

        public void SetLevels(List<ScriptableLevel> levelsDb) => _levelsDb = levelsDb; 
        public void SetTilePosition(Vector3Int position) => _position = position; 
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent<PlayerBuilder>(out var playerBuilder))
            {
                Debug.Log("El jugador ha entrado en la sala");
                levels = new List<ScriptableLevel>();

                levels = tileMapManager.FindLevelsWithEntrances(_levelsDb,tile.exit);
                
                Debug.Log(levels[UnityEngine.Random.Range(0, levels.Count)].levelIndex);
                actualLevel = levels[UnityEngine.Random.Range(0, levels.Count)].levelIndex;
            }
        }
    }
}
