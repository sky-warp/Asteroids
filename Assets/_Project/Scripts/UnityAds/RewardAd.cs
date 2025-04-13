using System;
using _Project.Scripts.Configs.Ads;
using R3;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;

namespace _Project.Scripts.UnityAds
{
    public class RewardAd : IInitializable, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        public readonly ReactiveProperty<bool> WasWatched = new(false);
        
        private string _adUnitID;
        private string _androidAdUnitID;
        private string _iOSAdUnitID;
        
        public RewardAd(AdsConfig config)
        {
            _androidAdUnitID = config.RewardAdAndroidID;
            _iOSAdUnitID = config.RewardAdIOSID;
        }

        public void Initialize()
        {
#if UNITY_ANDROID
            _adUnitID = _androidAdUnitID;

#elif UNITY_EDITOR
            _adUnitID = _androidAdUnitID;

#elif UNITY_IOS
            _adUnitID = _iOSAdUnitID;

#endif
        }

        public void LoadRewardAd()
        {
            Advertisement.Load(_adUnitID, this);
        }

        public void ShowRewardAd()
        {
            Advertisement.Show(_adUnitID, this);
            LoadRewardAd();
        }
        
        public void OnUnityAdsAdLoaded(string placementId)
        {
            if (placementId == _adUnitID)
            {
                Debug.Log("Ad Loaded");
            }
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            throw new Exception("Ad Failed: " + message);
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
        }

        public void OnUnityAdsShowStart(string placementId)
        {
        }

        public void OnUnityAdsShowClick(string placementId)
        {
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            WasWatched.Value = true;
        }
    }
}