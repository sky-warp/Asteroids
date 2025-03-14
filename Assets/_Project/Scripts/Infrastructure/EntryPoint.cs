using System;
using _Project.Scripts.Configs.GameConfigs;
using _Project.Scripts.InputService;
using _Project.Scripts.LevelBorder;
using _Project.Scripts.Player;
using _Project.Scripts.Projectiles.Ammo.Model;
using _Project.Scripts.Projectiles.Ammo.View;
using _Project.Scripts.Projectiles.Ammo.ViewModel;
using _Project.Scripts.Score.Model;
using _Project.Scripts.Score.View;
using _Project.Scripts.Score.ViewModel;
using _Project.Scripts.Spaceship.Model;
using _Project.Scripts.Spaceship.View;
using _Project.Scripts.Spaceship.ViewModel;
using _Project.Scripts.SpawnService;
using UnityEngine;
using R3;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class EntryPoint : IInitializable, IDisposable
    {
        private GameConfig _gameConfig;

        private Canvas _levelCanvas;

        private Transform _spaceshipStatsParent;

        private SpaceshipView _spaceship;
        private AmmoView _ammoView;
        private ScoreView _scoreView;

        private Transform[] _spawnPoints;

        private LevelColliderBorder _levelColliderBorder;

        private IInputable _inputManager;

        private SpaceshipViewModel _spaceshipViewModel;
        private AmmoViewModel _ammoViewModel;
        private ScoreViewModel _scoreViewModel;

        private ProjectileSpawnService _projectileSpawnService;
        private EnvironmentUnitSpawnService _environmentUnitSpawnService;

        private GameOverService.GameOverService _gameOverServiceService;

        private CoroutineManager.CoroutineManager _coroutineManager;
        
        private CompositeDisposable _disposable;

        [Inject]
        private void Construct(GameConfig gameConfig, Canvas levelCanvas, Transform spaceshipStatsParent,
            AmmoView ammoView, ScoreView scoreView, Transform[] spawnPoints, LevelColliderBorder levelColliderBorder,
            SpaceshipView spaceship, CoroutineManager.CoroutineManager coroutineManager)
        {
            _gameConfig = gameConfig;
            _levelCanvas = levelCanvas;
            _spaceshipStatsParent = spaceshipStatsParent;
            _ammoView = ammoView;
            _scoreView = scoreView;
            _spawnPoints = spawnPoints;
            _levelColliderBorder = levelColliderBorder;
            _spaceship = spaceship;
            _coroutineManager = coroutineManager;
        }

        public void Initialize()
        {
            _gameOverServiceService = new();
            _inputManager = new InputManager();
            _disposable = new();

            //var spaceship = Instantiate(_gameConfig.SpaceshipViewPrefab, _levelCanvas.transform);
            SpaceshipModel spaceshipModel = new SpaceshipModel(_gameConfig.SpaceshipConfig);
            _spaceshipViewModel = new SpaceshipViewModel(spaceshipModel, _gameOverServiceService,
                _spaceship.GetComponent<PlayerMovement>());

            _spaceship.Init(_spaceshipViewModel, _spaceshipStatsParent);
            _spaceship.GetComponent<PlayerMovement>().Init(_spaceshipViewModel.SpaceshipSpeedView.Value, _inputManager);

            _levelColliderBorder.Init(_spaceship);

            _gameOverServiceService.OnGameOver
                .Subscribe(_ => _spaceship.GetComponent<PlayerMovement>().GameOver())
                .AddTo(_disposable);
            _gameOverServiceService.OnGameOver
                .Subscribe(_ => _coroutineManager.StopAllCoroutines())
                .AddTo(_disposable);
            _gameOverServiceService.OnGameOver
                .Subscribe(_ => _inputManager.IsAvailable.Value = false)
                .AddTo(_disposable);

            _projectileSpawnService = new(_inputManager, _gameConfig.BulletPrefab, _gameConfig.LaserPrefab,
                _levelColliderBorder, _spaceship.transform, _gameOverServiceService, _levelCanvas);

            _environmentUnitSpawnService = new(_gameConfig.AsteroidBigPrefab, _gameConfig.AsteroidSmallPrefab,
                _gameConfig.UfoChaserPrefab, _levelCanvas.transform, _spaceship.transform,
                _levelColliderBorder, _spawnPoints,
                _gameOverServiceService, _levelCanvas);

            _coroutineManager.StartCoroutine(_environmentUnitSpawnService.SpawnBigAsteroids());
            _coroutineManager.StartCoroutine(_environmentUnitSpawnService.SpawnUfoChasers());

            AmmoModel ammoModel = new AmmoModel(_gameConfig.AmmoConfig);
            _ammoViewModel = new AmmoViewModel(ammoModel, _projectileSpawnService, _gameOverServiceService);
            _ammoView.Init(_ammoViewModel);

            ScoreModel scoreModel = new();
            _scoreViewModel = new ScoreViewModel(scoreModel, _environmentUnitSpawnService, _gameOverServiceService);
            _scoreView.Init(_scoreViewModel);
        }

        public void Dispose()
        {
            _disposable?.Dispose();

            _gameOverServiceService?.Dispose();

            _projectileSpawnService?.Dispose();
            _environmentUnitSpawnService?.Dispose();

            _spaceshipViewModel?.Dispose();
            _ammoViewModel?.Dispose();
            _scoreViewModel?.Dispose();
        }
    }
}