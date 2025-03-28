using System;
using _Project.Scripts.CoroutineManagers;
using _Project.Scripts.GameOverServices;
using _Project.Scripts.InputService;
using _Project.Scripts.Player;
using _Project.Scripts.Projectiles.Ammo.ViewModel;
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
        
        private IInputable _inputManager;

        private SpaceshipViewModel _spaceshipViewModel;
        private AmmoViewModel _ammoViewModel;
        private ScoreViewModel _scoreViewModel;
        
        private ProjectileSpawnService _projectileSpawnService;
        private EnvironmentUnitSpawnService _environmentUnitSpawnService;

        private DefaultGameStateService _defaultGameStateServiceService;

        private CoroutineManager _coroutineManager;
        
        private PlayerMovement _playerMovement;
        
        private CompositeDisposable _disposable = new();
        
        private GameEventManager _gameEventManager;
        
        private EntryPoint(
            SpaceShipStats statsParent,
            AmmoViewModel ammoViewModel,
            ScoreViewModel scoreViewModel, 
            SpaceshipViewModel spaceshipViewModel,
            PlayerMovement playerMovement,
            SpaceshipView spaceship, 
            CoroutineManager coroutineManager,
            DefaultGameStateService defaultGameStateService,
            IInputable inputManager,
            EnvironmentUnitSpawnService environmentUnitSpawnService,
            ProjectileSpawnService projectileSpawnService,
            GameEventManager gameEventManager
            )
        {
            _spaceshipStatsParent = statsParent;
            _ammoViewModel = ammoViewModel;
            _scoreViewModel = scoreViewModel;
            _spaceshipViewModel = spaceshipViewModel;
            _playerMovement = playerMovement;
            _spaceship = spaceship;
            _coroutineManager = coroutineManager;
            _defaultGameStateServiceService = defaultGameStateService;
            _inputManager = inputManager;
            _environmentUnitSpawnService = environmentUnitSpawnService;
            _projectileSpawnService = projectileSpawnService;
            _gameEventManager = gameEventManager;
        }

        public void Initialize()
        {
            _defaultGameStateServiceService.OnGameStart.OnNext(Unit.Default);
            
            _spaceship.Init(_spaceshipViewModel, _spaceshipStatsParent);
            _playerMovement.Init(_spaceshipViewModel.SpaceshipSpeedView.Value, _inputManager);
            
            _defaultGameStateServiceService.OnGameOver
                .Subscribe(_ => _spaceship.GetComponent<PlayerMovement>().GameOver())
                .AddTo(_disposable);
            _defaultGameStateServiceService.OnGameOver
                .Subscribe(_ => _coroutineManager.StopAllCoroutines())
                .AddTo(_disposable);
            _defaultGameStateServiceService.OnGameOver
                .Subscribe(_ => _inputManager.IsAvailable.Value = false)
                .AddTo(_disposable);

            _coroutineManager.StartCoroutine(_environmentUnitSpawnService.SpawnBigAsteroids());
            _coroutineManager.StartCoroutine(_environmentUnitSpawnService.SpawnUfoChasers());
            
            _gameEventManager.OnStartGame();
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