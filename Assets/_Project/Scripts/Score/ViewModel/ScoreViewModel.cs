using _Project.Scripts.Firebase;
using _Project.Scripts.GameOverServices;
using _Project.Scripts.SaveSystems;
using _Project.Scripts.Score.Model;
using _Project.Scripts.SpawnService;
using R3;

namespace _Project.Scripts.Score.ViewModel
{
    public class ScoreViewModel : Zenject.IInitializable
    {
        public readonly ReactiveProperty<int> CurrentScoreView = new();
        public readonly ReactiveProperty<bool> IsGameOver = new();

        private ScoreModel _scoreModel;

        private CompositeDisposable _disposable = new();

        private SceneManager.SceneManager _sceneManager;

        private ScoreSaveSystem _scoreSaveSystem;
        
        private EnvironmentUnitSpawnService _environmentUnitSpawnService;
        
        private DefaultGameStateService _defaultGameStateService;
        
        private FirebaseEventManager _firebaseEventManager;

        public ScoreViewModel(ScoreModel scoreModel, EnvironmentUnitSpawnService environmentUnitSpawnService,
            DefaultGameStateService defaultGameStateService, ScoreSaveSystem scoreSaveSystem,
            FirebaseEventManager firebaseEventManager)
        {
            _scoreModel = scoreModel;
            _environmentUnitSpawnService = environmentUnitSpawnService;
            _defaultGameStateService = defaultGameStateService;
            _scoreSaveSystem = scoreSaveSystem;
            _firebaseEventManager = firebaseEventManager;
            _sceneManager = new();
            
            ResetScore();
        }

        public void Initialize()
        {
            _scoreModel.CurrentScore
                .Subscribe(currentScore => CurrentScoreView.Value = currentScore)
                .AddTo(_disposable);

            CurrentScoreView
                .Subscribe(_scoreSaveSystem.SaveData.SaveHighScoreData)
                .AddTo(_disposable);

            _environmentUnitSpawnService.BigAsteroidScore
                .Subscribe(IncreaseScore)
                .AddTo(_disposable);
            _environmentUnitSpawnService.BigAsteroidScore
                .Where(bigAsteroid => bigAsteroid != 0)
                .Subscribe(_ => _firebaseEventManager.IncreaseBigAsteroidDestroyed())
                .AddTo(_disposable);


            _environmentUnitSpawnService.SmallAsteroidScore
                .Subscribe(IncreaseScore)
                .AddTo(_disposable);
            _environmentUnitSpawnService.SmallAsteroidScore
                .Where(smallAsteroid => smallAsteroid != 0)
                .Subscribe(_ => _firebaseEventManager.IncreaseSmallAsteroidDestroyed())
                .AddTo(_disposable);

    
            _environmentUnitSpawnService.UfoScore
                .Subscribe(IncreaseScore)
                .AddTo(_disposable);
            _environmentUnitSpawnService.UfoScore
                .Where(ufoScore => ufoScore != 0)
                .Subscribe(_ => _firebaseEventManager.IncreaseUfoDestroyed())
                .AddTo(_disposable);


            _defaultGameStateService.OnGameOver
                .Subscribe(_ => IsGameOver.Value = true)
                .AddTo(_disposable);
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