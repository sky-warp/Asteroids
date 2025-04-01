using _Project.Scripts.Firebase;
using _Project.Scripts.GameOverServices;
using _Project.Scripts.SaveSystems;
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

        private ScoreSaveSystem _scoreSaveSystem;

        public ScoreViewModel(ScoreModel scoreModel, EnvironmentUnitSpawnService environmentUnitSpawnService,
            DefaultGameStateService defaultGameStateService, ScoreSaveSystem scoreSaveSystem,
            FirebaseEventManager firebaseEventManager)
        {
            _scoreModel = scoreModel;
            _scoreSaveSystem = scoreSaveSystem;

            _scoreModel.CurrentScore
                .Subscribe(currentScore => CurrentScoreView.Value = currentScore)
                .AddTo(_disposable);

            CurrentScoreView
                .Subscribe(scoreSaveSystem.SaveData.SaveHighScoreData)
                .AddTo(_disposable);

            environmentUnitSpawnService.BigAsteroidScore
                .Subscribe(IncreaseScore)
                .AddTo(_disposable);
            environmentUnitSpawnService.BigAsteroidScore
                .Where(bigAsteroid => bigAsteroid != 0)
                .Subscribe(_ => firebaseEventManager.IncreaseBigAsteroidDestroyed())
                .AddTo(_disposable);


            environmentUnitSpawnService.SmallAsteroidScore
                .Subscribe(IncreaseScore)
                .AddTo(_disposable);
            environmentUnitSpawnService.SmallAsteroidScore
                .Where(smallAsteroid => smallAsteroid != 0)
                .Subscribe(_ => firebaseEventManager.IncreaseSmallAsteroidDestroyed())
                .AddTo(_disposable);


            environmentUnitSpawnService.UfoScore
                .Subscribe(IncreaseScore)
                .AddTo(_disposable);
            environmentUnitSpawnService.UfoScore
                .Where(ufoScore => ufoScore != 0)
                .Subscribe(_ => firebaseEventManager.IncreaseUfoDestroyed())
                .AddTo(_disposable);


            defaultGameStateService.OnGameOver
                .Subscribe(_ => IsGameOver.Value = true)
                .AddTo(_disposable);

            _sceneManager = new();

            ResetScore();
        }

        public void IncreaseScore(int score)
        {
            CurrentScoreView.Value += score;
        }

        public void ResetHighScoreView()
        {
            _scoreSaveSystem.ResetHighScore();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        public void OnRestartGame()
        {
            _sceneManager.RestartGame();
        }

        private void ResetScore()
        {
            _scoreModel.CurrentScore.Value = 0;
        }
    }
}