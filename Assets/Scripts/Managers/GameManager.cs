using Misc;
using UI;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Singleton Instance")]
        public static GameManager Instance;
        
        [Header("References")]
        [SerializeField] private WinLoseUI winLoseUI;

        [Header("Game Settings")]
        private GameState _currentGameState;
        private int _playerScore;
        
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
        }

        private void OnCorrectCollectibleCollected()
        {
            _playerScore++;
        }
        
        public void GameWin()
        {
            ChangeGameState(GameState.Win);
            winLoseUI.OnGameWin();
        }
        
        public void GameOver()
        {
            ChangeGameState(GameState.GameOver);
            winLoseUI.OnGameOver();
        }
        
        #region Helper Methods

        public GameState GetCurrentGameState()
        {
            return _currentGameState;
        }

        public int GetPlayerScore()
        {
            return _playerScore;
        }
        
        #endregion
        
    }
}