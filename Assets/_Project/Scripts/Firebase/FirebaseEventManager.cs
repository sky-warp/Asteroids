using _Project.Scripts.Infrastructure;
using Firebase.Analytics;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Firebase
{
    public class FirebaseEventManager : MonoBehaviour
    {
        private bool _isReady;
        private GameEventManager _eventManager;

        [Inject]
        private void Construct(GameEventManager gameEventManager)
        {
            _eventManager = gameEventManager;
            _eventManager.OnGameStart += SentGameStartEvent;
        }

        private void SentGameStartEvent()
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
        }

        public void ChangeReadyState()
        {
            _isReady = !_isReady;
        }

        private void OnDestroy()
        {
            _eventManager.OnGameStart -= SentGameStartEvent;
        }
    }
}