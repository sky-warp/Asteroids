using Firebase.Analytics;
using UnityEngine;

namespace _Project.Scripts.Firebase
{
    public class FirebaseEventManager : MonoBehaviour
    {
        private bool _isReady;

        public void SentGameStartEvent()
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
        }

        public void SentLaserUseEvent()
        {
            FirebaseAnalytics.LogEvent("LaserUsage", new Parameter("Status", "Was used"));
        }
        
        public void ChangeReadyState()
        {
            _isReady = !_isReady;
        }
    }
}