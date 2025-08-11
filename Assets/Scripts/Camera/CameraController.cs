using Cinemachine;
using Managers;
using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineClearShot _camera;
        
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
            _camera.Follow = target; 
        }
    }
}