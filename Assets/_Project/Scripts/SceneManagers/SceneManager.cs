namespace _Project.Scripts.SceneManagers
{
    public class SceneManager
    {
        public void LoadMainScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }

        public void RestartGame()
        {
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
        }
    }
}