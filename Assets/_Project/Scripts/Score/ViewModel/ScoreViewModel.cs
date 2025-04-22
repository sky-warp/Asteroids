using _Project.Scripts.Firebase;
using _Project.Scripts.GameOverServices;
using _Project.Scripts.InAppPurchase;
using _Project.Scripts.SaveSystems;
using _Project.Scripts.SceneManagers;
using _Project.Scripts.Score.Model;
using _Project.Scripts.SpawnService;
using _Project.Scripts.UnityAds;
using R3;

namespace _Project.Scripts.Score.ViewModel
{
    public class ScoreViewModel : Zenject.IInitializable
    {
        public IAPController IAPController { get; private set; }

        public readonly ReactiveProperty<int> CurrentScoreView = new();
        public readonly ReactiveProperty<bool> IsGameOver = new();
        public readonly ReactiveProperty<bool> IsGameResume = new();

        public IAdShowable AdManager { get; private set; }

        private ScoreModel _scoreModel;

        private CompositeDisposable _disposable = new();

        private SceneManager _sceneManager;

        private ScoreSaveSystem _scoreSaveSystem;

        private EnvironmentUnitSpawnService _environmentUnitSpawnService;

        private DefaultGameStateService _defaultGameStateService;

        private FirebaseEventManager _firebaseEventManager;

        public ScoreViewModel(ScoreModel scoreModel, EnvironmentUnitSpawnService environmentUnitSpawnService,
            DefaultGameStateService defaultGameStateService, ScoreSaveSystem scoreSaveSystem,
            FirebaseEventManager firebaseEventManager, SceneManager sceneManager, IAdShowable adManager,
            IAPController iapController)
        {
            _scoreModel = scoreModel;
            _environmentUnitSpawnService = environmentUnitSpawnService;
            _defaultGameStateService = defaultGameStateService;
            _scoreSaveSystem = scoreSaveSystem;
            _firebaseEventManager = firebaseEventManager;
            _sceneManager = sceneManager;
            AdManager = adManager;
            IAPController = iapController;

            ResetScore();
        }

        public void Initialize()
        {
            _scoreModel.CurrentScore
                .Subscribe(currentScore => CurrentScoreView.Value = currentScore)
                .AddTo(_disposable);

            CurrentScoreView
                .Subscribe(currentScore => _scoreSaveSystem.SaveData.SetHighScoreData(currentScore))
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
            _defaultGameStateService.OnGameResume
                .Subscribe(_ => IsGameOver.Value = false)
                .AddTo(_disposable);
            _defaultGameStateService.OnGameResume
                .Subscribe(_ => IsGameResume.Value = true)
                .AddTo(_disposable);

            AdManager.ShortAdWasWatched
                .Where(wasWatched => wasWatched)
                .Subscribe(_ => OnRestartGame())
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

        public void OnRestartGame()
        {
            _sceneManager.RestartGame();
        }

        private void ResetScore()
        {
            _scoreModel.CurrentScore.Value = 0;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}