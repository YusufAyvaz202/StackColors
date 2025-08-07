using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GoldUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _goldText;

        #region Unity Methods

        private void OnEnable()
        {
            EventManager.OnGoldCollected += UpdateGoldText;
        }
        
        private void OnDisable()
        {
            EventManager.OnGoldCollected -= UpdateGoldText;
        }

        #endregion

        private void UpdateGoldText()
        {
            _goldText.text = GameManager.Instance.GetPlayerGold().ToString();
        }
    }
}