using System;
using Interface;
using Managers;
using Misc;
using UnityEngine;

namespace Collectibles
{
    [RequireComponent(typeof(Rigidbody))]
    public class Collectible : MonoBehaviour, ICollectible
    {
        [Header("References")] 
        private MeshRenderer _meshRenderer;
        private Rigidbody _rigidbody;
        private Collider _collider;

        [Header("Settings")]
        [SerializeField] private CollectibleType _collectibleType;
        [SerializeField] private ColorType _currentColorType;
        [SerializeField] private Material _currentColorMaterial;

        private Material _colorMaterial;
        private ColorType _colorType;
        private bool _isCollected;

        [Header("Follow Settings")]
        [SerializeField] private Transform _followTransform;
        [SerializeField] private float _swayStrength = 0.5f;
        [SerializeField] private float _swaySpeed = 5f;
        [SerializeField] private float _dampening = 2f;

        private Vector3 _baseLocalPosition;
        private bool _basePositionSet;
        private Vector3 _currentVelocity;
        private Vector3 _lastParentPosition;
        private float _swayOffset;
        private bool isFollowing = true;

        #region Unity Methods

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _colorMaterial = _currentColorMaterial;
            _colorType = _currentColorType;

            _rigidbody.isKinematic = true;
        }
        
        private void Start()
        {
            if (_followTransform != null)
            {
                _lastParentPosition = _followTransform.position;
            }
        }

        private void OnEnable()
        {
            EventManager.OnFewerModeActive += ReadyForFewerMode;
            EventManager.OnFewerModeDisable += DisableFewerMode;
        }

        private void FixedUpdate()
        {
            Follow();
        }

        // Object pooling can be used here to reuse the collectible object instead of destroying it.
        private void OnTriggerEnter(Collider other)
        {
            if (_collectibleType != CollectibleType.Color)
            {
                Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            EventManager.OnFewerModeActive -= ReadyForFewerMode;
            EventManager.OnFewerModeDisable -= DisableFewerMode;
        }

        #endregion

        private void Follow()
        {
            if (_followTransform == null || !isFollowing) return;

            if (!_basePositionSet)
            {
                _baseLocalPosition = transform.localPosition;
                _basePositionSet = true;
            }

            Vector3 parentMovement = _followTransform.position - _lastParentPosition;
            float horizontalMovement = parentMovement.x;

            _swayOffset += -horizontalMovement * _swayStrength;
            _swayOffset = Mathf.Lerp(_swayOffset, 0f, Time.fixedDeltaTime * _dampening);

            Vector3 currentLocalPosition = transform.localPosition;
            Vector3 targetPosition = new Vector3(
                _baseLocalPosition.x + _swayOffset,
                _baseLocalPosition.y,
                _baseLocalPosition.z
            );

            transform.localPosition = new Vector3(
                Mathf.Lerp(currentLocalPosition.x, targetPosition.x, Time.fixedDeltaTime * _swaySpeed),
                _baseLocalPosition.y,
                _baseLocalPosition.z
            );

            _lastParentPosition = _followTransform.position;
        }

        public void Collect(Action<ColorType, CollectibleType> onCollected)
        {
            onCollected?.Invoke(_currentColorType, _collectibleType);
        }
        
        public void KickCollectible(Vector3 force)
        {
            transform.SetParent(null);
            _rigidbody.isKinematic = false;
            _collider.isTrigger = false;
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }

        #region Fewer Mode

        private void ReadyForFewerMode(Material colorMaterial, ColorType colorType)
        {
            if (_collectibleType != CollectibleType.Color) return;
            SetColor(colorMaterial);
            _currentColorType = colorType;
        }

        private void DisableFewerMode()
        {
            if (!_isCollected && _collectibleType == CollectibleType.Color)
            {
                SetColor(_colorMaterial);
                _currentColorType = _colorType;
            }
        }

        #endregion

        #region Helper Methods
        
        public Material GetColorMaterial()
        {
            return _currentColorMaterial;
        }

        public void SetFollowSettings(Transform followTransform, float swayStrength, float swaySpeed, float dampening)
        {
            _followTransform = followTransform;
            _swayStrength = swayStrength;
            _swaySpeed = swaySpeed;
            _dampening = dampening;
            if (_followTransform != null)
            {
                _lastParentPosition = _followTransform.position;
            }
        }

        public void UpdateBasePosition()
        {
            _baseLocalPosition = transform.localPosition;
            _basePositionSet = true;
        }

        public void DisableFollow()
        {
            isFollowing = false;
        }

        public void SetColor(Material colorMaterial)
        {
            _meshRenderer.sharedMaterial = colorMaterial;
        }

        public void SetIsCollected(bool isCollected)
        {
            _isCollected = isCollected;
        }
        
        #endregion
    }
}