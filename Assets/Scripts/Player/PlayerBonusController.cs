using Managers;
using Misc;
using UnityEngine;

namespace Player
{
    public class PlayerBonusController : MonoBehaviour
    {
        [Header("Settings")]
        private float _totalBonus;
        
        #region Unity Methods

        private void OnEnable()
        {
            EventManager.OnBonusActionPerformed += IncreaseBonus;
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
            if(GameManager.Instance.GetCurrentGameState() != GameState.Playing) return;
            _totalBonus += 0.2f;
        }

        #region Helper Methods

        public float GetTotalBonus()
        {
            return _totalBonus;
        }

        #endregion
    }
}