using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UnityAds
{
    public class AdsView : MonoBehaviour
    {
        [SerializeField] private Button ShowRewardedAdButton;
        
        [Inject]private AdManager _adManager;

        private void Start()
        {
            ShowRewardedAdButton.onClick.AddListener(_adManager.ShowRewardAd);
        }

        private void OnDestroy()
        {
            ShowRewardedAdButton.onClick.RemoveAllListeners();
        }
    }
}