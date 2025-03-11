using _Project.Scripts.Configs.AmmoConfigs;
using _Project.Scripts.Configs.SpaceshipConfigs;
using _Project.Scripts.Environment.EnvironmentUnitTypes;
using _Project.Scripts.InputService;
using _Project.Scripts.LevelBorder;
using _Project.Scripts.Player;
using _Project.Scripts.Projectiles.Ammo.Model;
using _Project.Scripts.Projectiles.Ammo.View;
using _Project.Scripts.Projectiles.Ammo.ViewModel;
using _Project.Scripts.Projectiles.ProjectileTypes;
using _Project.Scripts.Score.Model;
using _Project.Scripts.Score.View;
using _Project.Scripts.Score.ViewModel;
using _Project.Scripts.Spaceship.Model;
using _Project.Scripts.Spaceship.View;
using _Project.Scripts.Spaceship.ViewModel;
using _Project.Scripts.SpawnService;
using UnityEngine;
using R3;

namespace _Project.Scripts.Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private SpaceshipConfig _spaceshipConfig;
        [SerializeField] private AmmoConfig _ammoConfig;

        [SerializeField] private Canvas _levelCanvas;
        [SerializeField] private Transform _spaceshipStatsParent;

        [SerializeField] private SpaceshipView _spaceshipViewPrefab;
        [SerializeField] private AmmoView _ammoView;
        [SerializeField] private ScoreView _scoreView;

        [Header("Projectiles prefabs")] [SerializeField]
        private Bullet _bulletPrefab;

        [SerializeField] private Laser _laserPrefab;

        [Header("Environment objects prefabs")] [SerializeField]
        private AsteroidBig _asteroidBigPrefab;

        [SerializeField] private AsteroidSmall _asteroidSmallPrefab;
        [SerializeField] private UfoChaser _ufoChaserPrefab;

        [SerializeField] private Transform[] _spawnPoints;

        [SerializeField] private LevelColliderBorder _levelColliderBorder;

        private InputManager _inputManager;
        private SpaceshipViewModel _spaceshipViewModel;
        private AmmoViewModel _ammoViewModel;
        private ScoreViewModel _scoreViewModel;

        private ProjectileSpawnService _projectileSpawnService;
        private EnvironmentUnitSpawnService _environmentUnitSpawnService;

        private PauseGameService.PauseGameService _pauseGameService;

        private void Awake()
        {
            _pauseGameService = new();
            _inputManager = new();

            SpaceshipModel spaceshipModel = new SpaceshipModel(_spaceshipConfig);
            _spaceshipViewModel = new SpaceshipViewModel(spaceshipModel);
            var spaceship = Instantiate(_spaceshipViewPrefab, _levelCanvas.transform);
            spaceship.Init(_spaceshipViewModel, _spaceshipStatsParent, _pauseGameService);

            _pauseGameService.OnPause
                .Subscribe(_ => spaceship.GetComponent<PlayerMovement>().GameOver())
                .AddTo(this);
            _pauseGameService.OnPause
                .Subscribe(_ => StopAllCoroutines())
                .AddTo(this);

            _projectileSpawnService = new(_inputManager, _bulletPrefab, _laserPrefab,
                _levelColliderBorder, spaceship.transform, _pauseGameService, _levelCanvas);

            _environmentUnitSpawnService = new(_asteroidBigPrefab, _asteroidSmallPrefab,
                _ufoChaserPrefab, _levelCanvas.transform, spaceship.transform, _levelColliderBorder, _spawnPoints,
                _pauseGameService, _levelCanvas);

            StartCoroutine(_environmentUnitSpawnService.SpawnBigAsteroids());
            StartCoroutine(_environmentUnitSpawnService.SpawnUfoChasers());

            AmmoModel ammoModel = new AmmoModel(_ammoConfig);
            _ammoViewModel = new AmmoViewModel(ammoModel);
            _ammoView.Init(_ammoViewModel, _projectileSpawnService, _pauseGameService);

            ScoreModel scoreModel = new();
            _scoreViewModel = new ScoreViewModel(scoreModel);
            _scoreView.Init(_scoreViewModel, _environmentUnitSpawnService, _pauseGameService);
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