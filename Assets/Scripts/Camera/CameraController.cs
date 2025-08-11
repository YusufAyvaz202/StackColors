using Cinemachine;
using Managers;
using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineClearShot _camera;
        private CinemachineCameraOffset _cameraOffset;
        private bool _isChangeCamera;

        #region Unity Methods

        private void Awake()
        {
            _cameraOffset = _camera.GetComponent<CinemachineCameraOffset>();
        }

        private void OnEnable()
        {
            EventManager.ChangeCameraTarget += SetCameraFollowTarget;
        }

        private void FixedUpdate()
        {
            if (!_isChangeCamera) return;
            _cameraOffset.m_Offset = Vector3.Lerp(_cameraOffset.m_Offset, new Vector3(3, 10, 7), .7f * Time.fixedDeltaTime);
        }

        private void OnDisable()
        {
            EventManager.ChangeCameraTarget -= SetCameraFollowTarget;
        }

        #endregion


        private void SetCameraFollowTarget()
        {
            _isChangeCamera = true;
        }
    }
}