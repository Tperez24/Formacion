using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Enemies.Behaviour;
using Demo.LevelsManager.ChangeRoom;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using Utilities;
using Random = UnityEngine.Random;

namespace Demo.LevelsManager
{
   public class TileMapManager : MonoBehaviour
   {
      [SerializeField] private TileMaps tilemap;
      [SerializeField] private GameObject levelToSave;
      [SerializeField] private GameObject entrancePrefab;
      [SerializeField] private NavMeshSurface2d navMeshSurface2d;
      private List<SavedMapsWithTiles> _levelsLoaded;

      private int _loadOffset;

      private void Start() => LoadMap(tilemap.levelIndex);

      public ScriptableLevel FindLevelsWithEntrances(List<ScriptableLevel> levels, EntranceType.EntrancesTypes exit,ScriptableLevel actualLvl)
      {
         List<ScriptableLevel> list = new List<ScriptableLevel>();
         foreach (ScriptableLevel level in levels)
         {
            foreach (var map1 in level.maps.Where(map => map.type == TileMapsTypes.MapTypes.Entrances))
            {
               List<SavedTile> entranceTiles = map1.tiles.Where(tile => TileMatch(tile, exit)).ToList();
               if (entranceTiles.Count > 0) list.Add(level);
            }
         }

         if (list.Contains(actualLvl)) list.Remove(actualLvl);
         var levelToLoad = list[Random.Range(0, list.Count)];
         return levelToLoad;
      }
      
      public SavedTile GetTileAtPosition(Vector3Int pos, ScriptableLevel actualLevel)
      {
         var position = new Vector3Int(pos.x - _loadOffset, pos.y - _loadOffset);
         
         foreach (var map in actualLevel.maps)
         {
            if (map.type == TileMapsTypes.MapTypes.Entrances)
            {
               var savedTiles = map.tiles.Where(t => t.tileBase.GetType() == typeof(ChangeRoomTile)).ToList();
               var tile = savedTiles.Find(actualTile => actualTile.position == position);
               return tile;
            }
         }

         return null;
      }

      private static bool TileMatch(SavedTile tile,EntranceType.EntrancesTypes exit) => 
         tile.tileBase.GetType() == typeof(ChangeRoomTile) && tile.entrance == exit;

      IEnumerable<SavedTile> GetTilesFromMap(Tilemap map)
      {
         foreach (var pos in map.cellBounds.allPositionsWithin)
         {
            if (!map.HasTile(pos)) continue;
            var levelTile = map.GetTile<Tile>(pos);
            yield return new SavedTile
            {
               tileBase = levelTile,
               position = pos,
            };
         }
      }
   
      public void ClearMap()
      {
         foreach (var tileMapsTypes in tilemap.tileMapsTypesList) tileMapsTypes.map.ClearAllTiles();
      }

      public void LoadMap(int levelIndex)
      {
         var levelsDb = Resources.Load<LevelsDatabase>($"LevelsDB");
         var level = levelsDb.levels[levelIndex];
         
         if(level == null) return;
         
         foreach (var savedMap in level.maps)
         {
            switch (savedMap.type)
            {
               case TileMapsTypes.MapTypes.Entrances:
                  InitializeEntrances(savedMap, levelsDb, levelIndex);
                  break;
               
               case TileMapsTypes.MapTypes.Enemy:
                  InitializeEnemies(savedMap);
                  break;
               
               default:
               {
                  foreach (var tile in savedMap.tiles)
                  {
                     var tileMapsTypes = tilemap.tileMapsTypesList.Find(map => map.mapTypes == savedMap.type);
                     var position = new Vector3Int(tile.position.x + _loadOffset, tile.position.y + _loadOffset);
                     tileMapsTypes.map.SetTile(position, tile.tileBase);
                  }

                  break;
               }
            }
         }
         
         navMeshSurface2d.BuildNavMesh();
      }

      private void InitializeEnemies(SavedMapsWithTiles savedMap)
      {
         foreach (var tile in savedMap.tiles)
         {
            if(tile.tileBase.GetType() != typeof(EnemyTile)) continue;
            var enemiesMap =  tilemap.tileMapsTypesList.Find(map => map.mapTypes == TileMapsTypes.MapTypes.Enemy).map;
            var worldPos = Vector3Int.FloorToInt(enemiesMap.GetCellCenterWorld(tile.position));
            var worldPosUpdated = new Vector3Int(worldPos.x + _loadOffset, worldPos.y + _loadOffset);
            var parent = enemiesMap.transform;
            var enemy = Instantiate(tile.tileBase.GetAllProperties<GameObject>().First(),parent).GetComponent<BatBehaviour>();
                  
            enemy.Initialize(worldPosUpdated);
         }
      }

      private void InitializeEntrances(SavedMapsWithTiles savedMap,LevelsDatabase levelsDb, int levelIndex)
      {
         foreach (var tile in savedMap.tiles)
         {
            if(tile.tileBase.GetType() != typeof(ChangeRoomTile)) continue;
            var entrancesMap =  tilemap.tileMapsTypesList.Find(map => map.mapTypes == TileMapsTypes.MapTypes.Entrances).map;
            var worldPos = Vector3Int.FloorToInt(entrancesMap.GetCellCenterWorld(tile.position));
            var worldPosUpdated = new Vector3Int(worldPos.x + _loadOffset, worldPos.y + _loadOffset);
            var position = new Vector3Int(tile.position.x + _loadOffset, tile.position.y + _loadOffset);
            var parent = entrancesMap.transform;
            var entrance = Instantiate(tile.tileBase.GetAllProperties<GameObject>().First(),parent).GetComponent<CreateNewRoom>();
                  
            entrance.Initialize(levelsDb.levels,position,levelIndex);
            entrance.transform.position = worldPosUpdated;
         }
      }

      public ScriptableLevel GetActualLevel(List<ScriptableLevel> levelsDb,Vector3Int pos)
      {
         var position = new Vector3Int(pos.x - _loadOffset, pos.y - _loadOffset);
         foreach (var level in levelsDb)
         {
            var map = level.maps.Find(entranceMap => entranceMap.type == TileMapsTypes.MapTypes.Entrances);
            if (map.tiles.Find(tile => tile.position == position) != null) return level;
         }
         return null;
      }

      public void AddLevelOffset(int offset) => _loadOffset += offset;
      
      public void GetMapLayers()
      {
         var maps = levelToSave.GetComponentsInChildren<Tilemap>();
         var tileMap = new List<TileMapsTypes>(maps.Length);
         
         tileMap.AddRange(maps.Select(t => new TileMapsTypes() { map = t, mapTypes = GetMapTypeByName(t.name)}));

         tilemap.tileMapsTypesList = tileMap;
      }

#if UNITY_EDITOR

      public void SaveMap()
      {
         var newLevel = ScriptableObject.CreateInstance<ScriptableLevel>();
         newLevel.levelIndex = tilemap.levelIndex;
         newLevel.name = $"Level {tilemap.levelIndex }";

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

      public int GetLevelIndex() => tilemap.levelIndex;
      
#endif

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
            "Enemy" => TileMapsTypes.MapTypes.Enemy,
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
         Enemy
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