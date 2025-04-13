using System;
using _Project.Scripts.Configs.Ads;
using UnityEngine;
using UnityEngine.Advertisements;

namespace _Project.Scripts.UnityAds
{
    public class AdsInitializer : IUnityAdsInitializationListener
    {
        public RewardAd RewardAd { get; private set; }
        public ShortAd ShortAd {get; private set;}
        
        private string _gameID;
        private string _androidGameID;
        private string _iOSGameID;
        
        private bool _isTesting;

        public AdsInitializer(AdsConfig config, RewardAd rewardAd, ShortAd shortAd)
        {
            _androidGameID = config.AndroidGameID;
            _iOSGameID = config.IOSGameID;

            _isTesting = config.TestMode;
            
            RewardAd = rewardAd;
            ShortAd = shortAd;
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
            RewardAd.LoadRewardAd();
            ShortAd.LoadShortAd();
            Debug.Log("Ads were initialized");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            throw new Exception($"{message}");
        }
    }
}