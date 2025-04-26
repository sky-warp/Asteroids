using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.InAppPurchase.View
{
    public class IAPView : MonoBehaviour
    {
        [field: SerializeField] public Button NoAdsButton { get; private set; }
        [field: SerializeField] public Button ContinueGameButton { get; private set; }
    }
}