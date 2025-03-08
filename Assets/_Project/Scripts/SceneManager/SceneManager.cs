namespace _Project.Scripts.SceneManager
{
    public class SceneManager
    {
        public void RestartGame()
        {
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
        }
    }
}