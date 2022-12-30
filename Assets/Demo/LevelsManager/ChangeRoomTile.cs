#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.Tilemaps;

namespace Demo.LevelsManager
{
    public class ChangeRoomTile : Tile
    {
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