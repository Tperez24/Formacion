using System.Collections.Generic;
using Demo.Player.Player_Scripts.Player_Behaviour;
using Demo.Player.Player_Scripts.Player_Creator;
using UnityEngine;

namespace Demo.LevelsManager.ChangeRoom
{
    public class CreateNewRoom : MonoBehaviour
    {
        private List<ScriptableLevel> _levelsDb;
        private Vector3Int _position;
        private SavedTile _entrance;
        private SavedTile _connectedEntrance;
        private TileMapManager _tileMapManager;
        private Transform _player;
        private ScriptableLevel _levelToLoad,_actualLevel;

        private void Start() => _player = FindObjectOfType<PlayerController>().transform.parent;

        public void Initialize(List<ScriptableLevel> levelsDb, Vector3Int position)
        {
            _levelsDb = levelsDb;
            _position = position;
            _tileMapManager = FindObjectOfType<TileMapManager>();
            _actualLevel = _tileMapManager.GetActualLevel(_levelsDb,_position);
            _entrance = _tileMapManager.GetTileAtPosition(_position,_actualLevel);

            EntrancesManager.Instance.AddEntrance(this);
        }
        public void SetConnectedEntrance(SavedTile entranceToChain) => _connectedEntrance = entranceToChain;
        public SavedTile GetEntrance() => _entrance;
        public SavedTile GetConnectedEntrance() => _connectedEntrance;
        public ScriptableLevel GetActualLevel() => _actualLevel;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.TryGetComponent<PlayerBuilder>(out _) || !CanTeleport()) return;
            
            if (_connectedEntrance == null) GenerateNewLevel();
            else TeleportPlayer();
        }

        private bool CanTeleport() => Vector3.Distance(_player.position, transform.position) > 0.5f;

        private void GenerateNewLevel()
        {
            AddLevelOffset();
            SetNewLevel();
            ChainEntrances();
            TeleportPlayer();
        }

        private void AddLevelOffset() => _tileMapManager.AddLevelOffset(20);


        private void SetNewLevel()
        {
            (_levelToLoad) = _tileMapManager.FindLevelsWithEntrances(_levelsDb,_entrance.exit);
            _tileMapManager.LoadMap(_levelToLoad.levelIndex);
        }

        private void ChainEntrances() =>_connectedEntrance = EntrancesManager.Instance.ChainEntrances(_entrance,_entrance.entrance,_actualLevel,_levelToLoad.levelIndex);

        private void TeleportPlayer() => _player.position = EntrancesManager.Instance.GetEntrancePosition(_connectedEntrance);
    }
}
