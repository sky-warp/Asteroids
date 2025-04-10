using _Project.Scripts.Configs.Ads;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;

namespace _Project.Scripts.UnityAds
{
    public class FullscreenUnityAdUnit : IInitializable, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private string _androidAdID;
        private string _iOSAdID;
        
        private string _adUnitID;
        
        public FullscreenUnityAdUnit(AdsConfig config)
        {
            _androidAdID = config.AndroidFullscreenBannerUnitID;
            _iOSAdID = config.IOSFullscreenBannerUnitID;
        }

        public void Initialize()
        {
#if UNITY_ANDROID
            _adUnitID = _androidAdID;

#elif UNITY_EDITOR
            _adUnitID = _androidAdID;

#elif UNITY_IOS
            _adUnitID = _iOSAdID;

#endif
        }

        public void LoadAd()
        {
            Advertisement.Load(_adUnitID, this);
        }

        public void ShowAd()
        {
            Advertisement.Show(_adUnitID, this);
            LoadAd();
        }
        
        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log($"Add was loaded: {placementId}");
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
        }

        public void OnUnityAdsShowStart(string placementId)
        { }

        public void OnUnityAdsShowClick(string placementId)
        { }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            Debug.Log($"Show was completed: {placementId}");
        }
    }
}