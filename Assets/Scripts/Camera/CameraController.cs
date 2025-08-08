using Cinemachine;
using Managers;
using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineClearShot _camera;
        private Transform _target;
        #region Unity Methods

        private void OnEnable()
        {
            EventManager.ChangeCameraTarget += SetCameraFollowTarget;
        }
        
        private void OnDisable()
        {
            EventManager.ChangeCameraTarget -= SetCameraFollowTarget;
            CancelInvoke();
        }

        #endregion
        
        private void SetCameraFollowTarget(Transform target)
        {
            _target = target;
            InvokeRepeating(nameof(SetCamera), .2f,1f);
        }

        private void SetCamera()
        {
            _camera.ChildCameras[0].Follow = _target;
        }
        
    }
}