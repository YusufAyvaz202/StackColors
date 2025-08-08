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

        // Object pooling can be used here to reuse the collectible object instead of destroying it.
        private void OnTriggerEnter(Collider other)
        {
            if (_collectibleType != CollectibleType.Color)
            {
                Destroy(gameObject);
            }
            else if (_collectibleType == CollectibleType.Color)
            {
                FewerModeManager.Instance.RemoveCollectible(this);
            }
        }

        #endregion


        public void Collect(Action<ColorType, CollectibleType> onCollected)
        {
            onCollected?.Invoke(_currentColorType, _collectibleType);
        }

        public void ReadyForFewerMode(Material colorMaterial, ColorType colorType)
        {
            _meshRenderer.material = colorMaterial;
            _currentColorType = colorType;
        }

        public void DisableFewerMode()
        {
            _meshRenderer.material = _colorMaterial;
            _currentColorType = _colorType;
        }

        public void KickCollectible(Vector3 force)
        {
            transform.SetParent(null);
            _rigidbody.isKinematic = false;
            _collider.isTrigger = false;
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }

        public Material GetColorMaterial()
        {
            return _currentColorMaterial;
        }
    }
}