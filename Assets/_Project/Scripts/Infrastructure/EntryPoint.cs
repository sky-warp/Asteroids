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
        
        private InputManager _inputManager;
        private SpaceshipViewModel _spaceshipViewModel;
        private AmmoViewModel _ammoViewModel;
        private ScoreViewModel _scoreViewModel;

        private ProjectileSpawnService _projectileSpawnService;
        private EnvironmentUnitSpawnService _environmentUnitSpawnService;

        private PauseGameService.PauseGame _pauseGameService;

        private void Awake()
        {
            _pauseGameService = new();
            _inputManager = new();

            SpaceshipModel spaceshipModel = new SpaceshipModel(_gameConfig.SpaceshipConfig);

            var spaceship = Instantiate(_gameConfig.SpaceshipViewPrefab, _levelCanvas.transform);

            _spaceshipViewModel = new SpaceshipViewModel(spaceshipModel, _pauseGameService,
                spaceship.GetComponent<PlayerMovement>());

            spaceship.Init(_spaceshipViewModel, _spaceshipStatsParent);

            _pauseGameService.OnPause
                .Subscribe(_ => spaceship.GetComponent<PlayerMovement>().GameOver())
                .AddTo(this);
            _pauseGameService.OnPause
                .Subscribe(_ => StopAllCoroutines())
                .AddTo(this);

            _projectileSpawnService = new(_inputManager, _gameConfig.BulletPrefab, _gameConfig.LaserPrefab,
                _levelColliderBorder, spaceship.transform, _pauseGameService, _levelCanvas);

            _environmentUnitSpawnService = new(_gameConfig.AsteroidBigPrefab, _gameConfig.AsteroidSmallPrefab,
                _gameConfig.UfoChaserPrefab, _levelCanvas.transform, spaceship.transform,
                _levelColliderBorder, _spawnPoints,
                _pauseGameService, _levelCanvas);

            StartCoroutine(_environmentUnitSpawnService.SpawnBigAsteroids());
            StartCoroutine(_environmentUnitSpawnService.SpawnUfoChasers());

            AmmoModel ammoModel = new AmmoModel(_gameConfig.AmmoConfig);
            _ammoViewModel = new AmmoViewModel(ammoModel, _projectileSpawnService, _pauseGameService);
            _ammoView.Init(_ammoViewModel);

            ScoreModel scoreModel = new();
            _scoreViewModel = new ScoreViewModel(scoreModel, _environmentUnitSpawnService, _pauseGameService);
            _scoreView.Init(_scoreViewModel);
        }

        private void Update()
        {
            if (_pauseGameService.IsPaused.Value) return;

            if (Input.GetMouseButtonDown(0))
                _inputManager.OnLeftClick.OnNext(Unit.Default);

            if (Input.GetMouseButtonDown(1))
                _inputManager.OnRightClick?.OnNext(Unit.Default);
        }

        private void OnDestroy()
        {
            _pauseGameService.Dispose();

            _projectileSpawnService.Dispose();
            _environmentUnitSpawnService.Dispose();

            _spaceshipViewModel.Dispose();
            _ammoViewModel.Dispose();
            _scoreViewModel.Dispose();
        }
    }
}