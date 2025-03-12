using _Project.Scripts.GameOverService;
using _Project.Scripts.Score.Model;
using _Project.Scripts.SpawnService;
using R3;

namespace _Project.Scripts.Score.ViewModel
{
    public class ScoreViewModel
    {
        public readonly ReactiveProperty<int> CurrentScoreView = new();
        public readonly ReactiveProperty<bool> IsGameOver = new();

        private ScoreModel _scoreModel;
        private CompositeDisposable _disposable = new();
        private SceneManager.SceneManager _sceneManager;
        
        public ScoreViewModel(ScoreModel scoreModel, EnvironmentUnitSpawnService environmentUnitSpawnService,
            GameOverService.GameOverService gameOverService)
        {
            _scoreModel = scoreModel;

            _scoreModel.CurrentScore
                .Subscribe(currentScore => CurrentScoreView.Value = currentScore)
                .AddTo(_disposable);

            environmentUnitSpawnService.BigAsteroidScore
                .Subscribe(IncreaseScore)
                .AddTo(_disposable);
            environmentUnitSpawnService.SmallAsteroidScore
                .Subscribe(IncreaseScore)
                .AddTo(_disposable);
            environmentUnitSpawnService.UfoScore
                .Subscribe(IncreaseScore)
                .AddTo(_disposable);

            gameOverService.OnGameOver
                .Subscribe(_ => IsGameOver.Value = true)
                .AddTo(_disposable);
            
            _sceneManager = new();     
            
            ResetScore();
        }

        public void IncreaseScore(int score)
        {
            CurrentScoreView.Value += score;
        }

        private void ResetScore()
        {
            _scoreModel.CurrentScore.Value = 0;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
        
        public void OnRestartGame()
        {
            _sceneManager.RestartGame();
        }
    }
}