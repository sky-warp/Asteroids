using UnityEngine;

namespace _Project.Scripts.SceneManager
{
    public class SceneManager : MonoBehaviour
    {
        public void RestartGame()
        {
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
        }
    }
}