using _Project.Scripts.Score.Model;
using R3;
using UnityEngine;

namespace _Project.Scripts.SaveSystems
{
    public class ScoreSaveSystem
    {
        private int _highScore;

        private ScoreModel _scoreModel;

        public ScoreSaveSystem(ScoreModel scoreModel)
        {
            _scoreModel = scoreModel;

            _scoreModel.CurrentScore
                .Where(score => score > _highScore)
                .Do(score => _highScore = score)
                .Subscribe(_ => SetHighScore());
        }

        private void SetHighScore()
        {
            PlayerPrefs.SetInt("HighScore", _highScore);
            Debug.Log(PlayerPrefs.GetInt("HighScore"));
        }
    }
}