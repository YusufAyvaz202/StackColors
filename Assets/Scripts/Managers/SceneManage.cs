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

        #endregion
        
        public void LoadNextScene()
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (SceneManager.sceneCount < nextSceneIndex)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("No more scenes to load.");
            }
        }
        
        public void RestartCurrentScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}