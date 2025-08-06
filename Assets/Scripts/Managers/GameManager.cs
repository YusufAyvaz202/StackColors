using Misc;
using Player;
using UI;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Singleton Instance")]
        public static GameManager Instance;

        [Header("References")] 
        [SerializeField] private PlayerBonusController _playerBonusController;
        [SerializeField] private WinLoseUI _winLoseUI;
        [SerializeField] private BonusUI _bonusUI;

        [Header("Game Settings")]
        private GameState _currentGameState;
        private float _playerScore;
        
        /// <summary>
        /// Unity lifecycle methods for initialization and cleanup.
        /// </summary>
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
            DontDestroyOnLoad(gameObject);
            
            EventManager.OnCorrectCollectibleCollected += OnCorrectCollectibleCollected;
        }

        private void Start()
        {
            ChangeGameState(GameState.Waiting);
        }

        private void OnDisable()
        {
            EventManager.OnCorrectCollectibleCollected -= OnCorrectCollectibleCollected;
        }

        #endregion

        public void ChangeGameState(GameState newState)
        {
            _currentGameState = newState;
            EventManager.OnGameStateChanged?.Invoke(newState);
            
            CheckStates();
        }

        private void CheckStates()
        {
            switch (_currentGameState)
            {
                case GameState.BonusCalculation:
                    CalculateBonus();
                    break;
                case GameState.Win:
                    GameWin();  
                    break;
                case GameState.GameOver:
                    GameOver();
                    break;
            }
        }

        private void OnCorrectCollectibleCollected()
        {
            _playerScore++;
        }
        
        private void CalculateBonus()
        {
            if (_currentGameState != GameState.BonusCalculation) return;
            _playerScore *= _playerBonusController.GetTotalBonus();
            
            // This event for update the score UI.
            EventManager.OnCorrectCollectibleCollected?.Invoke();
            Debug.Log("Player Score after Bonus Calculation: " + _playerScore);
        }
        
        private void GameWin()
        {
            ChangeGameState(GameState.Win);
            _winLoseUI.OnGameWin();
        }
        
        private void GameOver()
        {
            ChangeGameState(GameState.GameOver);
            _winLoseUI.OnGameOver();
        }
        
        #region Helper Methods

        public GameState GetCurrentGameState()
        {
            return _currentGameState;
        }

        public int GetPlayerScore()
        {
            return (int)_playerScore;
        }

        public void SetActiveBonusUI()
        {
            _bonusUI.gameObject.SetActive(true);
        }
        
        public void UpdateBonusSliderValue(float value)
        {
            _bonusUI.UpdateBonusSlider(value);
        }
        
        #endregion
        
    }
}