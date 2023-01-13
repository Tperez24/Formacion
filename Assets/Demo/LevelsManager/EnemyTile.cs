#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.Tilemaps;

namespace Demo.LevelsManager
{
    public class EnemyTile : Tile
    {
        [MenuItem("Assets/Create/2D/Custom Tiles/Enemy Tile")]
        public static void CreateTile()
        {
            var path = EditorUtility.SaveFilePanelInProject("SaveChangeRoomTile", "Enemy", "Asset",
                "Tile that spawns enemies", "Assets");
            if (path == "") return;

            AssetDatabase.CreateAsset(CreateInstance<EnemyTile>(),path);
        }
    }
}
#endif