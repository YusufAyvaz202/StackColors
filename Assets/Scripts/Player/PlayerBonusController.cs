using Managers;
using Misc;
using UnityEngine;

namespace Player
{
    public class PlayerBonusController : MonoBehaviour
    {
        [Header("Settings")]
        public float _totalBonus;
        private bool _canIncreaseBonus;
        
        #region Unity Methods

        private void OnEnable()
        {
            EventManager.OnBonusActionPerformed += IncreaseBonus;
            EventManager.OnGameStateChanged += GameStateChanged;
        }
        
        private void OnDisable()
        {
            EventManager.OnBonusActionPerformed -= IncreaseBonus;
        }

        private void FixedUpdate()
        {
            _totalBonus -= Time.fixedDeltaTime * 2;
            _totalBonus = Mathf.Clamp(_totalBonus, 0f, 5f);
            GameManager.Instance.UpdateBonusSliderValue(_totalBonus);
        }

        #endregion
        
        private void IncreaseBonus()
        {
            if(!_canIncreaseBonus) return;
            _totalBonus += 0.2f;
        }

        private void GameStateChanged(GameState currentGameState)
        {
            if (currentGameState == GameState.Playing || currentGameState == GameState.FewerMode)
            {
                _canIncreaseBonus = true;
            }
            else
            {
                _canIncreaseBonus = false;
            }
        }

        #region Helper Methods

        public float GetTotalBonus()
        {
            return _totalBonus;
        }

        #endregion
    }
}