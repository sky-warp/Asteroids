using System;
using R3;
using UnityEngine;

namespace _Project.Scripts.SaveSystems
{
    public class ScoreSaveSystem : IDisposable
    {
        private int _highScore;
        public int HighScore => _highScore;

        private CompositeDisposable _disposables;

        public void SubscribeOnHighScore(ReactiveProperty<int> score)
        {
            score
                .Where(score => score > PlayerPrefs.GetInt("HighScore"))
                .Do(score => _highScore = score)
                .Subscribe(_ => UpdateHighScore());
        }

        private void UpdateHighScore()
        {
            PlayerPrefs.SetInt("HighScore", _highScore);
            Debug.Log(PlayerPrefs.GetInt("HighScore"));
        }
        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}