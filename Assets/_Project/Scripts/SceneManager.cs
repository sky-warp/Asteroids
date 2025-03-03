using UnityEngine;

namespace _Project.Scripts
{
    public class SceneManager : MonoBehaviour
    {
        public void RestartGame()
        {
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
            
            Time.timeScale = 1;
        }
    }
}