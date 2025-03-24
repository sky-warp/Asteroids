using System;
using R3;
using UnityEngine;

namespace _Project.Scripts.SaveSystems
{
    public class ScoreSaveSystem : IDisposable
    {
        public int _highScore;

        private readonly CompositeDisposable _disposable = new();

        public void SubscribeOnHighScore(ReactiveProperty<int> score)
        {
            score
                .Where(score => score > PlayerPrefs.GetInt("HighScore"))
                .Do(score => _highScore = score)
                .Subscribe(_ => UpdateHighScore())
                .AddTo(_disposable);
        }

        public void ResetHighScore()
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        private void UpdateHighScore()
        {
            PlayerPrefs.SetInt("HighScore", _highScore);

            Debug.Log($"High Score: {_highScore}");
        }
    }
}