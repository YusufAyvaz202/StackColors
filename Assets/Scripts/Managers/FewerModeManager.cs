using System.Collections.Generic;
using Collectibles;
using Misc;
using UnityEngine;

namespace Managers
{
    public class FewerModeManager : MonoBehaviour
    {
        [Header("Singleton Instance")]
        public static FewerModeManager Instance;

        [Header("References")] 
        [SerializeField] private List<Collectible> _collectibles = new List<Collectible>();

        [Header("Settings")]
        private Material _material;
        private ColorType _colorType;

        #region Unity Methods

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion
        
        [ContextMenu("Fewer Mode Activate")]
        public void FewerModeActivate()
        {
            foreach (var collectible in _collectibles)
            {
                collectible.ReadyForFewerMode(_material, _colorType);
            }
        }
        
        #region Helper Methods

        public void RemoveCollectible(Collectible collectible)    
        {
            if (_collectibles.Contains(collectible))
            {
                _collectibles.Remove(collectible);
            }
        }
        
        public void SetFewerModeMaterial(Material material, ColorType colorType)
        {
            _material = material;
            _colorType = colorType;
        }

        #endregion
    }
}