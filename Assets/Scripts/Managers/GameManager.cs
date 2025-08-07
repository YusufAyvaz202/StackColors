using Misc;
using Player;
using UI;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Singleton Instance")] 
        public static GameManager Instance;

        [Header("References")]
        private PlayerBonusController _playerBonusController;
        private WinLoseUI _winLoseUI;
        private BonusUI _bonusUI;

        [Header("Game Settings")]
        private GameState _currentGameState;
        private float _playerScore;
        private int _playerGold;

        [Inject]
        private void ZenjectSetup(PlayerBonusController playerBonusController, WinLoseUI winLoseUI, BonusUI bonusUI)
        {
            _playerBonusController = playerBonusController;
            _winLoseUI = winLoseUI;
            _bonusUI = bonusUI;
        }

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

            EventManager.OnCorrectCollectibleCollected += OnCorrectCollectibleCollected;
            EventManager.OnGoldCollected += () => _playerGold++;
        }

        private void Start()
        {
            ChangeGameState(GameState.Waiting);
        }

        private void OnDisable()
        {
            EventManager.OnCorrectCollectibleCollected -= OnCorrectCollectibleCollected;
            //EventManager.OnGoldCollected -= () => _playerGold++;
        }

        #endregion

        public void ChangeGameState(GameState newState)
        {
            _currentGameState = newState;
            EventManager.OnGameStateChanged?.Invoke(newState);
            
            Debug.Log("Game State Changed: " + newState);
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
                case GameState.FewerMode:
                    FewerModeManager.Instance.FewerModeActivate();
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
            ChangeGameState(GameState.Win);
        }

        private void GameWin()
        {
            SetActiveBonusUI(false);
            _winLoseUI.OnGameWin();
        }

        private void GameOver()
        {
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

        public int GetPlayerGold()
        {
            return _playerGold;
        }

        public void SetActiveBonusUI(bool isActive)
        {
            _bonusUI.gameObject.SetActive(isActive);
        }

        public void UpdateBonusSliderValue(float value)
        {
            _bonusUI.UpdateBonusSlider(value);
        }

        #endregion
    }
}