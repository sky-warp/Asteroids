using System;
using _Project.Scripts.Configs.Ads;
using R3;
using UnityEngine.Advertisements;
using Zenject;

namespace _Project.Scripts.UnityAds
{
    public class ShortAd : IInitializable, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        public readonly ReactiveProperty<bool> WasWatched = new(false);
        
        private string _adUnitID;
        private string _androidAdUnitID;
        private string _iOSAdUnitID;
        
        public ShortAd(AdsConfig config)
        {
            _androidAdUnitID = config.ShortAdAndroidID;
            _iOSAdUnitID = config.ShortAdIOSID;
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

        public void LoadShortAd()
        {
            Advertisement.Load(_adUnitID, this);
        }

        public void ShowShortAd()
        {
            Advertisement.Show(_adUnitID, this);
            LoadShortAd();
        }
        
        public void OnUnityAdsAdLoaded(string placementId)
        {
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            throw new Exception(message);
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            throw new Exception(message);
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