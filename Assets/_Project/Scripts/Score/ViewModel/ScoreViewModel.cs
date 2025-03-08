using _Project.Scripts.Score.Model;
using R3;

namespace _Project.Scripts.Score.ViewModel
{
    public class ScoreViewModel
    {
        public readonly ReactiveProperty<int> CurrentScoreView = new();
        
        private ScoreModel _scoreModel;
        private CompositeDisposable _disposable = new();

        public ScoreViewModel(ScoreModel scoreModel)
        {
            _scoreModel = scoreModel;

            _scoreModel.CurrentScore
                .Subscribe(currentScore => CurrentScoreView.Value = currentScore)
                .AddTo(_disposable);

            ResetScore();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
        
        public void IncreaseScore(int score)
        {
            CurrentScoreView.Value += score;
        }
        
        private void ResetScore()
        {
            _scoreModel.CurrentScore.Value = 0;
        }
    }
}