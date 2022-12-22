using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Demo
{
   public class TileMapManager : MonoBehaviour
   {
      [SerializeField] private TileMaps tilemap;
      [SerializeField] private GameObject levelToSave;

      public void SaveMap()
      {
         var newLevel = ScriptableObject.CreateInstance<ScriptableLevel>();
         newLevel.levelIndex = tilemap.levelIndex;
         newLevel.name = $"Level{tilemap.levelIndex }";

         newLevel.levels = new List<SavedMapsWithTiles>();

         foreach (var tileMap in tilemap.tileMapsTypesList)
         {
            var mapWithTiles = new SavedMapsWithTiles {type = tileMap.mapTypes, tiles = new List<SavedTile>()};
            var savedTiles = GetTilesFromMap(tileMap.map).ToList();
            
            foreach (var tile in savedTiles) mapWithTiles.tiles.Add(tile);
            
            newLevel.levels.Add(mapWithTiles);
            
         }

         ScriptableObjectUtility.SaveLevelFile(newLevel);
      }
      
      IEnumerable<SavedTile> GetTilesFromMap(Tilemap map)
      {
         foreach (var pos in map.cellBounds.allPositionsWithin)
         {
            if (!map.HasTile(pos)) continue;
            var levelTile = map.GetTile<Tile>(pos);
            yield return new SavedTile()
            {
               tileBase = levelTile,
               position = pos
            };
         }
      }
   
      public void ClearMap()
      {
         foreach (var tileMapsTypes in tilemap.tileMapsTypesList) tileMapsTypes.map.ClearAllTiles();
      }

      public void LoadMap()
      {
         var level = Resources.Load<ScriptableLevel>($"Level{tilemap.levelIndex}");
         if(level == null) return;
         
        
         foreach (var savedMap in level.levels)
         {
            foreach (var tile in savedMap.tiles)
            {
               var tileMapsTypes = tilemap.tileMapsTypesList.Find(map => map.mapTypes == savedMap.type);
               tileMapsTypes.map.SetTile(tile.position, tile.tileBase);
            }
         }
         
      }

      public void GetMapLayers()
      {
         var maps = levelToSave.GetComponentsInChildren<Tilemap>();
         var tileMap = new List<TileMapsTypes>(maps.Length);
         
         tileMap.AddRange(maps.Select(t => new TileMapsTypes() { map = t, mapTypes = GetMapTypeByName(t.name)}));

         tilemap.tileMapsTypesList = tileMap;
      }

      private TileMapsTypes.MapTypes GetMapTypeByName(string mapName)
      {
         return mapName switch
         {
            "Walls" => TileMapsTypes.MapTypes.Walls,
            "Decoration" => TileMapsTypes.MapTypes.Decoration,
            "Ground" => TileMapsTypes.MapTypes.Ground,
            "UnderGround" => TileMapsTypes.MapTypes.UnderGround,
            "UnderGround 1x1" => TileMapsTypes.MapTypes.UnderGround1X1,
            "TopDecoration" => TileMapsTypes.MapTypes.TopDecoration,
            "Decoration 1x1" => TileMapsTypes.MapTypes.Decoration1X1,
            _ => TileMapsTypes.MapTypes.Walls
         };
      }
   }

#if UNITY_EDITOR

   public static class ScriptableObjectUtility
   {
      public static void SaveLevelFile(ScriptableLevel level)
      {
         AssetDatabase.CreateAsset(level, $"Assets/Demo/Resources/{level.name}.asset");
      
         AssetDatabase.SaveAssets();
         AssetDatabase.Refresh();
      }
   }
   
   
   [Serializable]
   public struct TileMapsTypes
   {
      public MapTypes mapTypes;
      public Tilemap map;
      public enum MapTypes
      {
         Walls,
         TopDecoration,
         Decoration,
         Decoration1X1,
         Ground,
         UnderGround,
         UnderGround1X1,
      }
   }
   
   [Serializable]
   public struct TileMaps
   {
      public List<TileMapsTypes> tileMapsTypesList;
      public int levelIndex;
   }

#endif
}