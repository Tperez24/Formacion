﻿using System;
using System.Collections.Generic;
using Demo;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Demo
{
    public class ScriptableLevel : ScriptableObject
    {
        public int levelIndex;
        public List<SavedMapsWithTiles> levels;
    }
}

[Serializable]
public class SavedTile
{
    public Vector3Int position;
    public TileBase tileBase;
}

[Serializable]
public class SavedMapsWithTiles
{
    public List<SavedTile> tiles;
    public TileMapsTypes.MapTypes type;
}