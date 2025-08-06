using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneManage : MonoBehaviour
    {
        public static SceneManage Instance;

        #region Unity Methods

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            LoadNextScene();
        }

        #endregion

        public void LoadNextScene()
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            
            if(nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogWarning("No more scenes to load. Returning to the first scene.");
                nextSceneIndex = 0; // Loop back to the first scene
            }
            
            SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Additive);
        }

        public void RestartCurrentScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex, LoadSceneMode.Additive);
        }
    }
}