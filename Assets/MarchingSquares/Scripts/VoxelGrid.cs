using System;
using UnityEngine;

public class VoxelGrid : MonoBehaviour
{
   public GameObject voxelPrefab;
   
   public int resolution;
   public bool[] voxels;

   private float _voxelSize;

   private void Awake()
   {
      _voxelSize = 1f / resolution;
      voxels = new bool[resolution * resolution];
      
      for (int i = 0, y = 0; y < resolution; y++) 
      {
         for (var x = 0; x < resolution; x++, i++) 
         {
            CreateVoxel(i, x, y);
         }
      }
   }
   
   private void CreateVoxel (int i, int x, int y) 
   {
      var o = Instantiate(voxelPrefab, transform, true);
      o.transform.localPosition = new Vector3((x + 0.5f) * _voxelSize, (y + 0.5f) * _voxelSize);
      o.transform.localScale = Vector3.one * _voxelSize;
   }
}
