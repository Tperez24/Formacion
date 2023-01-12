using UnityEngine;

namespace MarchingSquares.Scripts
{
   [SelectionBase]
   public class VoxelGrid : MonoBehaviour
   {
      public GameObject voxelPrefab;

      public bool[] voxels;

      private int _resolution;
      private float _voxelSize;

      private Material[] _voxelMaterials;

      public void Initialize(int resolution, float size)
      {
         _resolution = resolution; 
         _voxelSize = size / _resolution;
         voxels = new bool[_resolution * _resolution];
         _voxelMaterials = new Material[voxels.Length];
      
         for (int i = 0, y = 0; y < _resolution; y++) 
         {
            for (var x = 0; x < _resolution; x++, i++) 
            {
               CreateVoxel(i, x, y);
            }
         }
         SetVoxelsColor();
      }

      private void SetVoxelsColor()
      {
         for (var i = 0; i < voxels.Length; i++) 
         {
            _voxelMaterials[i].color = voxels[i] ? Color.black : Color.white;
         }
      }

      private void CreateVoxel (int i, int x, int y) 
      {
         var o = Instantiate(voxelPrefab, transform, true);
         o.transform.localPosition = new Vector3((x + 0.5f) * _voxelSize, (y + 0.5f) * _voxelSize);
         o.transform.localScale = Vector3.one * _voxelSize * 0.9f;
         _voxelMaterials[i] = o.GetComponent<MeshRenderer>().material;
      }
      
      public void Apply (VoxelStencil voxelStencil)
      {
         var xStart = voxelStencil.XStart;
         var xEnd = voxelStencil.XEnd;
         if (xStart < 0) xStart = 0;
         if (xEnd >= _resolution) xEnd = _resolution - 1;
         
         var yStart = voxelStencil.YStart;
         var yEnd= voxelStencil.YEnd;
         if (yStart < 0) yStart = 0;
         if (yEnd >= _resolution) yEnd = _resolution - 1;

         for (var y = yStart; y <= yEnd; y++)
         {
            var i = y * _resolution + xStart;
            
            for (var x = xStart; x <= xEnd; x++, i++)
            {
               voxels[i] = voxelStencil.Apply(x,y);
            }
         }
         
         SetVoxelsColor();
      }
   }
}
