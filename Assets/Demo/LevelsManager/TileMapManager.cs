using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Demo.LevelsManager
{
   public class TileMapManager : MonoBehaviour
   {
      [SerializeField] private TileMaps tilemap;
      [SerializeField] private GameObject levelToSave;

      public List<ScriptableLevel> FindLevelsWithEntrances(List<ScriptableLevel> levels, EntranceType.EntrancesTypes exit)
      {
         var levelsThatMatch = new List<ScriptableLevel>();
         foreach (var level in levels)
         {
            foreach (var map in level.maps.Where(map => map.type == TileMapsTypes.MapTypes.Entrances))
            {
               var entranceTiles = map.tiles.Where(tile => TileMatch(tile,exit)).ToList();
               if(entranceTiles.Count > 0) levelsThatMatch.Add(level);
            }
         }

         return levelsThatMatch;
      }

      public SavedTile GetTileAtPosition(Vector3Int pos, List<ScriptableLevel> actualLevel)
      {
         foreach (var tile in actualLevel[tilemap.levelIndex].maps.Where(map => map.type == TileMapsTypes.MapTypes.Entrances))
         {
            var savedTiles = tile.tiles.Where(t => t.tileBase.GetType() == typeof(ChangeRoomTile)).ToList();
            return savedTiles.Find(actualTile => actualTile.position == pos);
         }
       
         return null;
      }
      private static bool TileMatch(SavedTile tile,EntranceType.EntrancesTypes exit)
      {
         return tile.tileBase.GetType() == typeof(ChangeRoomTile) && tile.entrance == exit;
      }

#if UNITY_EDITOR
      public void SaveMap()
      {
         var newLevel = ScriptableObject.CreateInstance<ScriptableLevel>();
         newLevel.levelIndex = tilemap.levelIndex;
         newLevel.name = $"Level{tilemap.levelIndex }";

         newLevel.maps = new List<SavedMapsWithTiles>();

         foreach (var tileMap in tilemap.tileMapsTypesList)
         {
            var mapWithTiles = new SavedMapsWithTiles {type = tileMap.mapTypes, tiles = new List<SavedTile>()};
            var savedTiles = GetTilesFromMap(tileMap.map).ToList();
            
            foreach (var tile in savedTiles){ mapWithTiles.tiles.Add(tile);}
            
            newLevel.maps.Add(mapWithTiles);
            
         }

         ScriptableObjectUtility.SaveLevelFile(newLevel);
      }
#endif
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
         
        
         foreach (var savedMap in level.maps)
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
            "ColliderMap" => TileMapsTypes.MapTypes.ColliderMap,
            "Entrances" => TileMapsTypes.MapTypes.Entrances,
            _ => TileMapsTypes.MapTypes.Walls
         };
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
         ColliderMap,
         Entrances,
      }
   }
   
   [Serializable]
   public struct TileMaps
   {
      public List<TileMapsTypes> tileMapsTypesList;
      public int levelIndex;
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
   
   

#endif
}