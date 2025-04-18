using R3;

namespace _Project.Scripts.SceneManagers
{
    public class SceneManager
    {
        public Observable<Unit> SceneChanged => _onSceneChange.AsObservable();       
        
        private readonly Subject<Unit> _onSceneChange = new();
        
        public void LoadMainScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }

        public void RestartGame()
        {
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
            
            _onSceneChange?.OnNext(Unit.Default);
        }
    }
}