#if UNITY_EDITOR
using Demo.LevelsManager.ChangeRoom;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Demo.LevelsManager
{
    public class ChangeRoomTile : Tile
    {
        public LevelsDatabase levelsDb;

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            if (go == null) return base.StartUp(position, tilemap, go);
            
            var entrance = go.AddComponent<CreateNewRoom>();
            entrance.SetLevels(levelsDb.levels);
            entrance.SetTilePosition(position);

            return base.StartUp(position, tilemap, go);
        }

        [MenuItem("Assets/Create/2D/Custom Tiles/Change Room Tile")]
        public static void CreateTile()
        {
            var path = EditorUtility.SaveFilePanelInProject("SaveChangeRoomTile", "ChangeRoom", "Asset",
                "Save tile that stores new rooms", "Assets");
            if (path == "") return;

            AssetDatabase.CreateAsset(CreateInstance<ChangeRoomTile>(),path);
        }
    }
}
#endif