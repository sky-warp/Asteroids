using System;
using R3;

namespace _Project.Scripts.SceneManagers
{
    public class SceneManager
    {
        public readonly Subject<Unit> OnSceneChange = new();
        
        public void LoadMainScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }

        public void RestartGame()
        {
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
            
            OnSceneChange?.OnNext(Unit.Default);
        }
    }
}