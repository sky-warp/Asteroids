using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Firebase
{
    public class FirebaseInstaller : MonoBehaviour
    {
        public event Action OnFirebaseInitialized;

        private FirebaseEventManager _firebaseEventManager;

        [Inject]
        private void Construct(FirebaseEventManager firebaseEventManager)
        {
            _firebaseEventManager = firebaseEventManager;
        }

        private void Start()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                OnFirebaseStatusRecived(task);
            });
            
            OnFirebaseInitialized += _firebaseEventManager.ChangeReadyState;
        }

        private void OnFirebaseStatusRecived(Task<DependencyStatus> task)
        {
            try
            {
                if (!task.IsCompletedSuccessfully)
                    throw new Exception("Failed to resolve all Firebase status", task.Exception);

                var status = task.Result;

                if (status != DependencyStatus.Available)
                    throw new Exception($"Failed to resolve all Firebase status : {status}");

                Debug.Log($"Successfully resolved all Firebase status : {status}");
                OnFirebaseInitialized?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void OnDestroy()
        {
            OnFirebaseInitialized -= _firebaseEventManager.ChangeReadyState;
        }
    }
}