using System;
using _Project.Scripts.CoroutineManagers;
using _Project.Scripts.GameOverServices;
using _Project.Scripts.InputService;
using _Project.Scripts.LevelBorder;
using _Project.Scripts.Player;
using _Project.Scripts.Projectiles.Ammo.View;
using _Project.Scripts.Projectiles.Ammo.ViewModel;
using _Project.Scripts.SaveSystems;
using _Project.Scripts.Score.View;
using _Project.Scripts.Score.ViewModel;
using _Project.Scripts.Spaceship.View;
using _Project.Scripts.Spaceship.ViewModel;
using _Project.Scripts.SpawnService;
using R3;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class EntryPoint : IInitializable, IDisposable
    {
        private SpaceShipStats _spaceshipStatsParent;

        private SpaceshipView _spaceship;
        private AmmoView _ammoView;
        private ScoreView _scoreView;
        
        private LevelColliderBorder _levelColliderBorder;

        private IInputable _inputManager;

        private SpaceshipViewModel _spaceshipViewModel;
        private AmmoViewModel _ammoViewModel;
        private ScoreViewModel _scoreViewModel;
        
        private ProjectileSpawnService _projectileSpawnService;
        private EnvironmentUnitSpawnService _environmentUnitSpawnService;

        private DefaultGameOverService _defaultGameOverServiceService;

        private CoroutineManager _coroutineManager;
        
        private PlayerMovement _playerMovement;
        
        private CompositeDisposable _disposable = new();

        private ScoreSaveSystem _scoreSaveSystem = new();
        
        private EntryPoint(
            SpaceShipStats statsParent,
            AmmoViewModel ammoViewModel,
            AmmoView ammoView, 
            ScoreViewModel scoreViewModel, 
            ScoreView scoreView, 
            LevelColliderBorder levelColliderBorder,
            PlayerMovement playerMovement,
            SpaceshipViewModel spaceshipViewModel,
            SpaceshipView spaceship, 
            CoroutineManager coroutineManager,
            DefaultGameOverService defaultGameOverService,
            IInputable inputManager,
            EnvironmentUnitSpawnService environmentUnitSpawnService,
            ProjectileSpawnService projectileSpawnService,
            ScoreSaveSystem scoreSaveSystem
            )
        {
            _spaceshipStatsParent = statsParent;
            _ammoViewModel = ammoViewModel;
            _ammoView = ammoView;
            _scoreViewModel = scoreViewModel;
            _scoreView = scoreView;
            _levelColliderBorder = levelColliderBorder;
            _playerMovement = playerMovement;
            _spaceshipViewModel = spaceshipViewModel;
            _spaceship = spaceship;
            _coroutineManager = coroutineManager;
            _defaultGameOverServiceService = defaultGameOverService;
            _inputManager = inputManager;
            _environmentUnitSpawnService = environmentUnitSpawnService;
            _projectileSpawnService = projectileSpawnService;
            _scoreSaveSystem = scoreSaveSystem;
        }

        public void Initialize()
        {
            _spaceship.Init(_spaceshipViewModel, _spaceshipStatsParent);
            _playerMovement.Init(_spaceshipViewModel.SpaceshipSpeedView.Value, _inputManager);

            _levelColliderBorder.Init(_spaceship);

            _defaultGameOverServiceService.OnGameOver
                .Subscribe(_ => _spaceship.GetComponent<PlayerMovement>().GameOver())
                .AddTo(_disposable);
            _defaultGameOverServiceService.OnGameOver
                .Subscribe(_ => _coroutineManager.StopAllCoroutines())
                .AddTo(_disposable);
            _defaultGameOverServiceService.OnGameOver
                .Subscribe(_ => _inputManager.IsAvailable.Value = false)
                .AddTo(_disposable);

            _coroutineManager.StartCoroutine(_environmentUnitSpawnService.SpawnBigAsteroids());
            _coroutineManager.StartCoroutine(_environmentUnitSpawnService.SpawnUfoChasers());
            
            _scoreSaveSystem.SubscribeOnHighScore(_scoreViewModel.CurrentScoreView);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            
            _projectileSpawnService?.Dispose();
            _environmentUnitSpawnService?.Dispose();

            _spaceshipViewModel?.Dispose();
            _ammoViewModel?.Dispose();
            _scoreViewModel?.Dispose();
        }
    }
}