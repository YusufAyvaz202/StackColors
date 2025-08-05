using Misc;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Singleton Instance")]
        public static GameManager Instance;

        [Header("Game Settings")]
        private GameState _currentGameState;
        
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
        }

        private void Start()
        {
            ChangeGameState(GameState.Playing);
        }

        #endregion

        private void ChangeGameState(GameState newState)
        {
            _currentGameState = newState;
        }


        #region Helper Methods

        public GameState GetCurrentGameState()
        {
            return _currentGameState;
        }

        #endregion
        
    }
}