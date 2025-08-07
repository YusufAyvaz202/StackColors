using System;
using Interface;
using Managers;
using Misc;
using UnityEngine;

namespace Collectibles
{
    public class Collectible : MonoBehaviour, ICollectible
    {
        [Header("References")]
        private MeshRenderer _meshRenderer;
        
        [Header("Settings")] 
        [SerializeField] private CollectibleType _collectibleType;
        [SerializeField] private ColorType _currentColorType;
        [SerializeField] private Material _currentColorMaterial;
        
        private Material _colorMaterial;
        private ColorType _colorType;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _colorMaterial = _currentColorMaterial;
            _colorType = _currentColorType;
        }

        public void Collect(Action<ColorType, CollectibleType> onCollected)
        {
            onCollected?.Invoke(_currentColorType, _collectibleType);
        }

        public Material GetColorMaterial()
        {
            return _currentColorMaterial;
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
    }
}