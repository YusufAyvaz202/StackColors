using System;
using Interface;
using Misc;
using UnityEngine;

namespace Collectible
{
    public class Collectible : MonoBehaviour, ICollectible
    {
        [Header("Settings")]
        [SerializeField] private ColorType _colorType;
        [SerializeField] private Color _colorMaterial;

        private void Awake()
        {
            GetComponent<MeshRenderer>().material.color = _colorMaterial;
        }

        public void Collect(Action<ColorType> onCollected)
        {
            onCollected?.Invoke(_colorType);
        }
    }
}