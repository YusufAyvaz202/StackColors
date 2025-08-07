using DG.Tweening;
using Managers;
using Misc;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private GameObject _blackBackgroundObject;
        [SerializeField] private GameObject _pausePopup;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _resumeButton;

        [Header("Settings")]
        [SerializeField] private float _animationDuration = 0.3f;
        private Image _blackBackgroundImage;
        private RectTransform _winPopupTransform;
        private RectTransform _losePopupTransform;

        private void Awake()
        {
            _blackBackgroundImage = _blackBackgroundObject.GetComponent<Image>();
            _winPopupTransform = _pausePopup.GetComponent<RectTransform>();

            _pauseButton.onClick.AddListener(OnGamePause);
            _resumeButton.onClick.AddListener(OnGameResume);
        }

        private void OnGamePause()
        {
            GameManager.Instance.ChangeGameState(GameState.Paused);

            _blackBackgroundObject.SetActive(true);
            _pausePopup.SetActive(true);
            _resumeButton.gameObject.SetActive(true);

            _blackBackgroundImage.DOFade(0.8f, _animationDuration).SetEase(Ease.Linear);
            _winPopupTransform.DOScale(1.5f, _animationDuration).SetEase(Ease.OutBack);
        }

        private void OnGameResume()
        {
            GameManager.Instance.ChangeGameState(GameState.Playing);

            _blackBackgroundImage.DOFade(1f, _animationDuration).SetEase(Ease.Linear);
            _winPopupTransform.DOScale(1.0f, _animationDuration).SetEase(Ease.OutBack);

            _blackBackgroundObject.SetActive(false);
            _pausePopup.SetActive(false);
            _resumeButton.gameObject.SetActive(false);
        }
    }
}