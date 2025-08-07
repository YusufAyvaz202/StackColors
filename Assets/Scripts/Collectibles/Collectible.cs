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
        [SerializeField] private ColorType _colorType;
        [SerializeField] private Material _colorMaterial;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void Collect(Action<ColorType, CollectibleType> onCollected)
        {
            onCollected?.Invoke(_colorType, _collectibleType);
        }

        public Material GetColorMaterial()
        {
            return _colorMaterial;
        }

        public void ReadyForFewerMode(Material colorMaterial, ColorType colorType)
        {
            _meshRenderer.material = colorMaterial;
            _colorType = colorType;
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