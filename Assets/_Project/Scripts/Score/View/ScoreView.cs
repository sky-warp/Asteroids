using _Project.Scripts.Score.ViewModel;
using _Project.Scripts.SpawnService;
using R3;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Score.View
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private EnvironmentUnitSpawnService _environmentUnitSpawnService;

        private ScoreViewModel _scoreViewModel;

        public void Init(ScoreViewModel scoreViewModel)
        {
            _scoreViewModel = scoreViewModel;
            
            _environmentUnitSpawnService.OnScoreChanged
                .Subscribe(_scoreViewModel.IncreaseScore)
                .AddTo(this);
            
            _scoreViewModel.CurrentScoreView
                .Subscribe(UpdateScoreText)
                .AddTo(this);
        }

        private void UpdateScoreText(int score)
        {
            _scoreText.text = $"SCORE: {score.ToString()}";
        }
    }
}