using _Project.Scripts.Firebase;
using _Project.Scripts.SaveSystems;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class Bootstrapper : IInitializable
    {
        private ScoreSaveSystem _scoreSaveSystem;
        private FirebaseInstaller _firebaseInstaller;

        public Bootstrapper(ScoreSaveSystem scoreSaveSystem,
            FirebaseInstaller firebaseInstaller)
        {
            _scoreSaveSystem = scoreSaveSystem;
            _firebaseInstaller = firebaseInstaller;
            
            Debug.Log("Score save system loaded");
        }

        public void Initialize()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }
    }
}