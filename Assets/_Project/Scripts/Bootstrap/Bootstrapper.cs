using _Project.Scripts.AudioSystems;
using _Project.Scripts.Firebase;
using _Project.Scripts.GameOverServices;
using _Project.Scripts.ParticleSystems;
using _Project.Scripts.SaveSystems;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class Bootstrapper : IInitializable
    {
        public Bootstrapper(ScoreSaveSystem scoreSaveSystem,
            FirebaseInstaller firebaseInstaller,
            DefaultAudioManager defaultAudioManager,
            DefaultVisualEffectSystem defaultVisualEffectSystem,
            DefaultGameStateService gameStateService)
        {
        }

        public void Initialize()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }
    }
}