using Misc;
using UnityEngine;

namespace Managers
{
    public class FewerModeManager : MonoBehaviour
    {
        [Header("Singleton Instance")]
        public static FewerModeManager Instance;

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
        
        public void FewerModeActivate()
        {
            ChangeFewerModeMaterial();
            Invoke(nameof(FewerModeDeactivate), Conts.FewerMode.FEWER_MODE_DURATION);
        }
        
        public void ChangeFewerModeMaterial()
        {
            EventManager.OnFewerModeActive?.Invoke(_material, _colorType);
        }
        
        private void FewerModeDeactivate()
        {
            EventManager.OnFewerModeChanged?.Invoke(0f);
            GameManager.Instance.ChangeGameState(GameState.Playing);
           
            EventManager.OnFewerModeDisable?.Invoke();
        }
        
        #region Helper Methods

        public void SetFewerModeMaterial(Material material, ColorType colorType)
        {
            _material = material;
            _colorType = colorType;
        }

        #endregion
    }
}