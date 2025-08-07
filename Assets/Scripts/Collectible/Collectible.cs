using System;
using Interface;
using Misc;
using UnityEngine;

namespace Collectible
{
    public class Collectible : MonoBehaviour, ICollectible
    {
        [Header("Settings")] 
        [SerializeField] private CollectibleType _collectibleType;
        [SerializeField] private ColorType _colorType;
        [SerializeField] private Material _colorMaterial;

        public void Collect(Action<ColorType, CollectibleType> onCollected)
        {
            onCollected?.Invoke(_colorType, _collectibleType);
        }

        public Material GetColorMaterial()
        {
            return _colorMaterial;
        }

        public ColorType GetColorType()
        {
            return _colorType;
        }
        
    }
}