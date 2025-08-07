using Managers;
using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FewerUI : MonoBehaviour
    {
        [SerializeField] private Slider _fewerSlider;
        [SerializeField] private TextMeshProUGUI _fewerText;

        #region Unity Methods

        private void OnEnable()
        {
            EventManager.OnFewerModeChanged += SetFewerSliderValue;
        }
        
        private void OnDisable()
        {
            EventManager.OnFewerModeChanged -= SetFewerSliderValue;
        }

        #endregion
        
        private void SetFewerSliderValue(float value)
        {
            _fewerSlider.value = value;
            if (Mathf.Approximately(_fewerSlider.value, Conts.FewerMode.FewerModeCount))
            {
                FewerModeActive();
            }
            else if (_fewerSlider.value == 0f)
            {
                DisableFewerUI();
            }
        }
        
        private void FewerModeActive()
        {
            _fewerText.gameObject.SetActive(true);
        }
        
        private void DisableFewerUI()
        {
            _fewerText.gameObject.SetActive(false);
        }
    }
}