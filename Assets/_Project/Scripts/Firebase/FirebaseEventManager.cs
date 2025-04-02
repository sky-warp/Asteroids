using Firebase.Analytics;
using UnityEngine;

namespace _Project.Scripts.Firebase
{
    public class FirebaseEventManager : MonoBehaviour
    {
        private bool _isReady;
        private int _bulletUsageCount;
        private int _laserUsageCount;
        private int _bigAsteroidDestroyedCount;
        private int _smallAsteroidDestroyedCount;
        private int _ufoDestroyedCount;

        public void SentGameStartEvent()
        {
            if (_isReady)
                FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
        }

        public void SentGameEndEvent()
        {
            if (_isReady)
            {
                FirebaseAnalytics.LogEvent(
                    FirebaseAnalytics.EventSelectContent,
                    new Parameter("game_over", "game_over"),
                    new Parameter("bullet_used", _bulletUsageCount),
                    new Parameter("laser_used", _laserUsageCount),
                    new Parameter("big_asteroid_destroyed", _bigAsteroidDestroyedCount),
                    new Parameter("small_asteroid_destroyed", _smallAsteroidDestroyedCount),
                    new Parameter("ufo_destroyed", _ufoDestroyedCount)
                );

                ResetCountStats();
            }
        }

        public void SentLaserUseEvent()
        {
            if (_isReady)
                FirebaseAnalytics.LogEvent("LaserUsage", new Parameter("Status", "Was used"));
        }

        public void ChangeReadyState()
        {
            _isReady = !_isReady;
        }

        public void IncreaseBulletUsage()
        {
            _bulletUsageCount++;
        }

        public void IncreaseLaserUsage()
        {
            _laserUsageCount++;
        }

        public void IncreaseBigAsteroidDestroyed()
        {
            _bigAsteroidDestroyedCount++;
        }

        public void IncreaseSmallAsteroidDestroyed()
        {
            _smallAsteroidDestroyedCount++;
        }

        public void IncreaseUfoDestroyed()
        {
            _ufoDestroyedCount++;
        }

        private void ResetCountStats()
        {
            _bulletUsageCount = default;
            _laserUsageCount = default;
            _bigAsteroidDestroyedCount = default;
            _smallAsteroidDestroyedCount = default;
            _ufoDestroyedCount = default;
        }
    }
}