using System;
using Cysharp.Threading.Tasks;
using Firebase.RemoteConfig;
using UnityEngine;

namespace _Project.Scripts.Firebase
{
    public class RemoteConfigInitializer
    {
        private RemoteData _remoteData;

        public RemoteConfigInitializer(RemoteData remoteData)
        {
            _remoteData = remoteData;
        }

        public async UniTask LoadRemoteData()
        {
            await CheckRemoteConfig();
        }
        
        private async UniTask CheckRemoteConfig()
        {
            await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            await FetchComplete();
        }

        private async UniTask FetchComplete()
        {
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            var info = remoteConfig.Info;

            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                throw new Exception("RemoteConfig failed to fetch");
            }

            await remoteConfig.ActivateAsync();
            
            string confData = remoteConfig.GetValue("AsteroidData").StringValue;
            
            JsonUtility.FromJsonOverwrite(confData, _remoteData);
        }
    }
}