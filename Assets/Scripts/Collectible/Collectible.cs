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
        [SerializeField] private Material _colorMaterial;

        #region Unity Methods

        private void Awake()
        {
            GetComponent<MeshRenderer>().material = _colorMaterial;
        }

        #endregion

        public void Collect(Action<ColorType> onCollected)
        {
            onCollected?.Invoke(_colorType);
        }
    }
}