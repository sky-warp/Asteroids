using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using UnityEngine;

namespace _Project.Scripts.Firebase
{
    public class FirebaseInstaller : MonoBehaviour
    {
        private void Start()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(OnFirebaseStatusRecived);
        }

        private void OnFirebaseStatusRecived(Task<DependencyStatus> task)
        {
            try
            {
                if(!task.IsCompletedSuccessfully)
                    throw new Exception("Failed to resolve all Firebase status", task.Exception);
                
                var status  = task.Result;
                
                if(status != DependencyStatus.Available)
                    throw new Exception($"Failed to resolve all Firebase status : {status}");
                
                Debug.Log($"Successfully resolved all Firebase status : {status}");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}