using System;
using _Project.Scripts.InAppPurchase.Products;
using Cysharp.Threading.Tasks;
using Firebase.RemoteConfig;
using UnityEngine;

namespace _Project.Scripts.Firebase
{
    public class RemoteConfigInitializer : IRemoteDataLoadable
    {
        private RemoteData _remoteData;
        private NoAdsProductData _noAdsProductData;

        public RemoteConfigInitializer(RemoteData remoteData, NoAdsProductData noAdsProductData)
        {
            _remoteData = remoteData;
            _noAdsProductData = noAdsProductData;
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
            string adsData = remoteConfig.GetValue("NoAdsProduct").StringValue;
            
            JsonUtility.FromJsonOverwrite(confData, _remoteData);
            JsonUtility.FromJsonOverwrite(adsData, _noAdsProductData);
        }
    }
}