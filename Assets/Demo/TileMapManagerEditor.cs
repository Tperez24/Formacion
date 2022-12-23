#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Demo
{
    [CustomEditor(typeof(TileMapManager)),CanEditMultipleObjects]
    public class TileMapManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            var script = (TileMapManager)target;

            if (GUILayout.Button("SaveMap")) script.SaveMap();
            if (GUILayout.Button("ClearMap")) script.ClearMap();
            if (GUILayout.Button("GetLevelMaps")) script.GetMapLayers();
            if (GUILayout.Button("LoadMap")) script.LoadMap();
        }
    }
}
#endif