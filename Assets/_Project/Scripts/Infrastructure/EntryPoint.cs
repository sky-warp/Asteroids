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
using _Project.Scripts.Spaceship.ViewModel;
using _Project.Scripts.SpawnService;
using UnityEngine;
using R3;

namespace _Project.Scripts.Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;

        [SerializeField] private Canvas _levelCanvas;
        
        [SerializeField] private Transform _spaceshipStatsParent;
        
        [SerializeField] private AmmoView _ammoView;
        [SerializeField] private ScoreView _scoreView;
        
        [SerializeField] private Transform[] _spawnPoints;
        
        [SerializeField] private LevelColliderBorder _levelColliderBorder;
        
        private IInputable _inputManager;
        private SpaceshipViewModel _spaceshipViewModel;
        private AmmoViewModel _ammoViewModel;
        private ScoreViewModel _scoreViewModel;

        private ProjectileSpawnService _projectileSpawnService;
        private EnvironmentUnitSpawnService _environmentUnitSpawnService;

        private GameOverService.GameOverService _gameOverServiceService;

        private void Awake()
        {
            _gameOverServiceService = new();
            _inputManager = new InputManager();

            SpaceshipModel spaceshipModel = new SpaceshipModel(_gameConfig.SpaceshipConfig);

            var spaceship = Instantiate(_gameConfig.SpaceshipViewPrefab, _levelCanvas.transform);

            _spaceshipViewModel = new SpaceshipViewModel(spaceshipModel, _gameOverServiceService,
                spaceship.GetComponent<PlayerMovement>());

            spaceship.Init(_spaceshipViewModel, _spaceshipStatsParent);
            spaceship.GetComponent<PlayerMovement>().Init(_spaceshipViewModel.SpaceshipSpeedView.Value, _inputManager);

            _gameOverServiceService.OnGameOver
                .Subscribe(_ => spaceship.GetComponent<PlayerMovement>().GameOver())
                .AddTo(this);
            _gameOverServiceService.OnGameOver
                .Subscribe(_ => StopAllCoroutines())
                .AddTo(this); 
            _gameOverServiceService.OnGameOver
                .Subscribe(_ => _inputManager.IsAvailable.Value = false)
                .AddTo(this);

            _projectileSpawnService = new(_inputManager, _gameConfig.BulletPrefab, _gameConfig.LaserPrefab,
                _levelColliderBorder, spaceship.transform, _gameOverServiceService, _levelCanvas);

            _environmentUnitSpawnService = new(_gameConfig.AsteroidBigPrefab, _gameConfig.AsteroidSmallPrefab,
                _gameConfig.UfoChaserPrefab, _levelCanvas.transform, spaceship.transform,
                _levelColliderBorder, _spawnPoints,
                _gameOverServiceService, _levelCanvas);

            StartCoroutine(_environmentUnitSpawnService.SpawnBigAsteroids());
            StartCoroutine(_environmentUnitSpawnService.SpawnUfoChasers());

            AmmoModel ammoModel = new AmmoModel(_gameConfig.AmmoConfig);
            _ammoViewModel = new AmmoViewModel(ammoModel, _projectileSpawnService, _gameOverServiceService);
            _ammoView.Init(_ammoViewModel);

            ScoreModel scoreModel = new();
            _scoreViewModel = new ScoreViewModel(scoreModel, _environmentUnitSpawnService, _gameOverServiceService);
            _scoreView.Init(_scoreViewModel);
        }

        private void OnDestroy()
        {
            _gameOverServiceService.Dispose();

            _projectileSpawnService.Dispose();
            _environmentUnitSpawnService.Dispose();

            _spaceshipViewModel.Dispose();
            _ammoViewModel.Dispose();
            _scoreViewModel.Dispose();
        }
    }
}