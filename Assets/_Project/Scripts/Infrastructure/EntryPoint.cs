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

        private Transform _spaceshipStatsParent;

        private SpaceshipView _spaceship;
        private AmmoView _ammoView;
        private ScoreView _scoreView;
        
        private LevelColliderBorder _levelColliderBorder;

        private IInputable _inputManager;

        private SpaceshipViewModel _spaceshipViewModel;
        private AmmoViewModel _ammoViewModel;
        private ScoreViewModel _scoreViewModel;

        private SpaceshipModel _spaceshipModel;
        
        private ProjectileSpawnService _projectileSpawnService;
        private EnvironmentUnitSpawnService _environmentUnitSpawnService;

        private GameOverService.GameOverService _gameOverServiceService;

        private CoroutineManager.CoroutineManager _coroutineManager;
        
        private PlayerMovement _playerMovement;
        
        private CompositeDisposable _disposable = new();

        [Inject]
        private void Construct(
            GameConfig gameConfig, 
            Transform spaceshipStatsParent,
            AmmoView ammoView, 
            ScoreView scoreView, 
            LevelColliderBorder levelColliderBorder,
            PlayerMovement playerMovement,
            SpaceshipModel spaceshipModel,
            SpaceshipViewModel spaceshipViewModel,
            SpaceshipView spaceship, 
            CoroutineManager.CoroutineManager coroutineManager,
            GameOverService.GameOverService gameOverService,
            IInputable inputManager,
            EnvironmentUnitSpawnService environmentUnitSpawnService,
            ProjectileSpawnService projectileSpawnService)
        {
            _gameConfig = gameConfig;
            _spaceshipStatsParent = spaceshipStatsParent;
            _ammoView = ammoView;
            _scoreView = scoreView;
            _levelColliderBorder = levelColliderBorder;
            _playerMovement = playerMovement;
            _spaceshipModel = spaceshipModel;
            _spaceshipViewModel = spaceshipViewModel;
            _spaceship = spaceship;
            _coroutineManager = coroutineManager;
            _gameOverServiceService = gameOverService;
            _inputManager = inputManager;
            _environmentUnitSpawnService = environmentUnitSpawnService;
            _projectileSpawnService = projectileSpawnService;
        }

        public void Initialize()
        {
            _spaceship.Init(_spaceshipViewModel, _spaceshipStatsParent);
            _playerMovement.Init(_spaceshipViewModel.SpaceshipSpeedView.Value, _inputManager);

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