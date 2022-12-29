using System;
using Cinemachine;
using Demo.Player.Player_Scripts.Player_Creator;
using UnityEngine;

namespace Demo.CameraBehaviour
{
    public class SetFollower : MonoBehaviour
    {
        public CinemachineVirtualCamera virtualCamera;

        private void OnEnable() => GameInitializator.OnPlayerSpawn += SetCameraTarget;

        private void SetCameraTarget(object sender, Transform target) => virtualCamera.m_Follow = target;

        private void OnDisable() => GameInitializator.OnPlayerSpawn -= SetCameraTarget;
    }
}
