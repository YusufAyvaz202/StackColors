using Managers;
using Misc;
using UnityEngine;

namespace Player
{
    public class PlayerBonusController : MonoBehaviour
    {
        [Header("Settings")]
        public float totalBonus;
        
        #region Unity Methods

        private void OnEnable()
        {
            EventManager.OnBonusActionPerformed += GetBonus;
        }
        
        private void OnDisable()
        {
            EventManager.OnBonusActionPerformed -= GetBonus;
        }

        private void FixedUpdate()
        {
            totalBonus -= Time.fixedDeltaTime;
            totalBonus = Mathf.Clamp(totalBonus, 0f, 5f);
            GameManager.Instance.UpdateBonusSliderValue(totalBonus);
        }

        #endregion
        
        private void GetBonus()
        {
            if(GameManager.Instance.GetCurrentGameState() != GameState.Playing) return;
            totalBonus += 0.2f;
        }

        #region Helper Methods

        public float GetTotalBonus()
        {
            return totalBonus;
        }

        #endregion
    }
}