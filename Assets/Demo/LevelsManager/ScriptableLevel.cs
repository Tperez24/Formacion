using System;
using System.Collections.Generic;
using Demo;
using Demo.DrawIfProperty;
using Demo.LevelsManager;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Demo
{
    public class ScriptableLevel : ScriptableObject
    {
        public Sprite icon;
        public int levelIndex;
        public List<SavedMapsWithTiles> maps;
    }
}

[Serializable]
public class SavedTile
{
    public Vector3Int position;
    public TileBase tileBase;
    
    public EntranceType.EntrancesTypes entrance;
    public EntranceType.EntrancesTypes exit;
}

[Serializable]
public class SavedMapsWithTiles
{
    public List<SavedTile> tiles;
    public TileMapsTypes.MapTypes type;
}