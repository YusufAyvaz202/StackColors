using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneManage : MonoBehaviour
    {
        public static SceneManage Instance;
        private int _currentSceneIndex;
        [SerializeField] private GameObject BaseScenePrefab;

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
            
            SceneManager.sceneLoaded += OnSceneLoaded;
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
                nextSceneIndex = 1; // Loop back to the first scene
            }
            
            SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Single);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.buildIndex == 0) return;
            Instantiate(BaseScenePrefab);
        }

        public void RestartCurrentScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex, LoadSceneMode.Single);
        }
    }
}