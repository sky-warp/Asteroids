using UnityEngine;

namespace _Project.Scripts.Configs.Ads
{
    [CreateAssetMenu(fileName = "AdsConfig", menuName = "Create New Ads Config/ AdsConfig")]
    public class AdsConfig : ScriptableObject
    {
        [field: SerializeField] public string IOSGameID { get; private set; }
        [field: SerializeField] public string AndroidGameID { get; private set; }
        [field: SerializeField] public string RewardAdAndroidID { get; private set; }
        [field: SerializeField] public string RewardAdIOSID { get; private set; }
        [field: SerializeField] public bool TestMode { get; private set; }
    }
}