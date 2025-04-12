using System;
using _Project.Scripts.Configs.Ads;
using UnityEngine;
using UnityEngine.Advertisements;

namespace _Project.Scripts.UnityAds
{
    public class AdsInitializer : IUnityAdsInitializationListener
    {
        private string _gameID;
        private string _androidGameID;
        private string _iOSGameID;

        private RewardAd _rewardAd;
        
        private bool _isTesting;

        public AdsInitializer(AdsConfig config, RewardAd rewardAd)
        {
            _androidGameID = config.AndroidGameID;
            _iOSGameID = config.IOSGameID;

            _isTesting = config.TestMode;
            
            _rewardAd = rewardAd;
        }

        public void InitializeAds()
        {
#if UNITY_ANDROID
            _gameID = _androidGameID;

#elif UNITY_EDITOR
            _gameID = _androidGameID;

#elif UNITY_IOS
            _gameID = _iOSGameID;

#endif
            if (!Advertisement.isInitialized && Advertisement.isSupported)
                Advertisement.Initialize(_gameID, _isTesting, this);
        }

        public void OnInitializationComplete()
        {
            _rewardAd.LoadRewardAd();
            Debug.Log("Ads were initialized");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            throw new Exception($"{message}");
        }
    }
}