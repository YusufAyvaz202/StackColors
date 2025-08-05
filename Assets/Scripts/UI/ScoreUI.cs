using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        #region Unity Methods

        private void OnEnable()
        {
            EventManager.OnCorrectCollectibleCollected += UpdateScoreText;
        }
        
        private void OnDisable()
        {
            EventManager.OnCorrectCollectibleCollected -= UpdateScoreText;
        }
        
        #endregion
        
        private void UpdateScoreText()
        {
            _scoreText.text = GameManager.Instance.GetPlayerScore().ToString();
        }
    }
}