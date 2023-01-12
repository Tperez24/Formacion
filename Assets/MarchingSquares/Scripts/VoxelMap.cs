﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarchingSquares.Scripts
{
    public class VoxelMap : MonoBehaviour
    {
        public float size = 2f;

        public int voxelResolution = 8;
        public int chunkResolution = 2;

        public VoxelGrid voxelGridPrefab;
        
        private VoxelGrid[] _chunks;
        private float _halfSize, _voxelSize, _chunkSize;

        private InputAction _leftClick;
        private int _fillTypeIndex,_radiusIndex;
        
        
        private static readonly string[] FillTypeNames = { "Filled", "Empty" };
        private static readonly string[] RadiusNames = {"0", "1", "2", "3", "4", "5"};

        private void Awake()
        {
            var master = new Master();
            master.Enable();

            _leftClick = master.SaveGame.Mouse;
            
            _halfSize = size * 0.5f;
            _chunkSize = size / chunkResolution;
            _voxelSize = _chunkSize / voxelResolution;

            _chunks = new VoxelGrid[chunkResolution * chunkResolution];
            
            for (int i = 0, y = 0; y < chunkResolution; y++) 
            {
                for (var x = 0; x < chunkResolution; x++, i++) 
                {
                    CreateChunk(i, x, y);
                }
            }

            var boxColl = gameObject.AddComponent<BoxCollider>();
            boxColl.size = new Vector3(size, size);
        }

        private void OnEnable()
        {
            _leftClick.performed += MousePressed;
        }

        private void MousePressed(InputAction.CallbackContext obj)
        {
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hitInfo)) return;
            if (hitInfo.collider.gameObject == gameObject) 
            {
                    EditVoxels(transform.InverseTransformPoint(hitInfo.point));
            }
        }

        private void CreateChunk (int i, int x, int y) 
        {
            var chunk = Instantiate(voxelGridPrefab, transform, true);
            chunk!.Initialize(voxelResolution, _chunkSize);
            chunk.transform.localPosition = new Vector3(x * _chunkSize - _halfSize, y * _chunkSize - _halfSize);
            _chunks[i] = chunk;
        }

        private void EditVoxels(Vector3 point)
        {
            var centerX = (int)((point.x + _halfSize) / _voxelSize);
            var centerY = (int)((point.y + _halfSize) / _voxelSize);

            var xStart = (centerX - _radiusIndex) / voxelResolution;
            var xEnd = (centerX + _radiusIndex) / voxelResolution;
            if (xStart < 0) xStart = 0;
            if (xEnd >= chunkResolution) xEnd = chunkResolution - 1; 
            
            var yStart = (centerY - _radiusIndex) / voxelResolution;
            var yEnd = (centerY + _radiusIndex) / voxelResolution;
            if (yStart < 0) yStart = 0;
            if (yEnd >= chunkResolution) yEnd = chunkResolution - 1;

            var activeStencil = new VoxelStencil();
            activeStencil.Initialize(_fillTypeIndex == 0, _radiusIndex);

            var voxelYOffset = yStart * voxelResolution;
            for (var y = yStart; y <= yEnd; y++) 
            {
                var i = y * chunkResolution + xStart;
                var voxelXOffset = xStart * voxelResolution;
                
                for (var x = xStart; x <= xEnd; x++, i++) 
                {
                    activeStencil.SetCenter(centerX - voxelXOffset, centerY - voxelYOffset);
                    _chunks[i].Apply(activeStencil);
                    voxelXOffset += voxelResolution;
                }
                
                voxelYOffset += voxelResolution;
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(4f, 4f, 150f, 500f));
            GUILayout.Label("Fill Type");
            _fillTypeIndex = GUILayout.SelectionGrid(_fillTypeIndex, FillTypeNames, FillTypeNames.Length);
            GUILayout.Label("Radius");
            _radiusIndex = GUILayout.SelectionGrid(_radiusIndex, RadiusNames, RadiusNames.Length);
            GUILayout.EndArea();
        }
    }
}