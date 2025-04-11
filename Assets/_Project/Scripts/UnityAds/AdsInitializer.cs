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

        private bool _isTesting;

        private AdsConfig _config;

        public AdsInitializer(AdsConfig config)//other types of ad to load after initialization
        {
            _config = config;

            _androidGameID = _config.AndroidGameID;
            _iOSGameID = _config.IOSGameID;

            _isTesting = config.TestMode;
        }

        public void InitializeAds()
        {
#if UNITY_ANDROID
            _gameID = _config.AndroidGameID;

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
            Debug.Log("Ads were initialized");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            throw new Exception($"{message}");
        }
    }
}