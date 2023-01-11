using System;
using System.Collections.Generic;
using Demo.LevelsManager;
using Demo.SaveManager;
using ObjectManagement.Scripts;
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
public class SavedTile : ISaver
{
    public Vector3Int position;
    public TileBase tileBase;
    //public Sprite sprite;
  
    public EntranceType.EntrancesTypes entrance;
    public EntranceType.EntrancesTypes exit;
    
    public void Save(GameDataWriter writer)
    {
        //Guardo posición y sprite
        writer.Write(position);
        /*var pixels = sprite.texture.GetPixels();
        writer.Write(pixels.Length);
        foreach (var pixel in pixels)
        {
            writer.Write(new Vector4(pixel.r,pixel.g,pixel.b,pixel.a));
        }*/
    }

    public void Load(GameDataReader reader)
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public class SavedMapsWithTiles
{
    public List<SavedTile> tiles;
    public TileMapsTypes.MapTypes type;
}