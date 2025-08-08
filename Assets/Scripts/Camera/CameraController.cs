using System;
using Cinemachine;
using Managers;
using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;

        #region Unity Methods

        private void OnEnable()
        {
            EventManager.ChangeCameraTarget += SetCameraFollowTarget;
        }
        
        private void OnDisable()
        {
            EventManager.ChangeCameraTarget -= SetCameraFollowTarget;
        }

        #endregion
        
        private void SetCameraFollowTarget(Transform target)
        {
            if (_camera != null && target != null)
            {
                _camera.Follow = target;
                _camera.LookAt = target;
            }
            else
            {
                Debug.LogWarning("Camera or target is null. Cannot set camera follow target.");
            }
        }
        
    }
}