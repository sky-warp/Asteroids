using _Project.Scripts.Configs.GameConfigs;
using _Project.Scripts.Infrastructure;
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
using Zenject;

namespace _Project.Scripts.Installers
{
    public class SceneMainInstaller : MonoInstaller
    {
        [SerializeField] private Canvas _levelCanvas;

        [SerializeField] private Transform _spaceshipStatsParent;

        [SerializeField] private AmmoView _ammoView;
        [SerializeField] private ScoreView _scoreView;

        [SerializeField] private Transform[] _spawnPoints;

        [SerializeField] private LevelColliderBorder _levelColliderBorder;

        [Inject]
        private readonly GameConfig _gameConfig;
        
        public override void InstallBindings()
        {

            Container
                .BindInterfacesTo<EntryPoint>()
                .AsSingle();

            Container
                .Bind<GameConfig>()
                .FromInstance(_gameConfig)
                .AsSingle();

            Container
                .Bind<Canvas>()
                .FromInstance(_levelCanvas)
                .AsSingle();

            Container
                .Bind<Transform>()
                .FromInstance(_spaceshipStatsParent)
                .AsSingle();

            Container
                .Bind<Transform[]>()
                .FromInstance(_spawnPoints)
                .AsSingle();

            Container
                .Bind<LevelColliderBorder>()
                .FromInstance(_levelColliderBorder)
                .AsSingle();

            Container
                .Bind<GameOverService.GameOverService>()
                .AsSingle();

            Container
                .Bind<IInputable>()
                .To<InputManager>()
                .AsSingle();

            var spaceship = Container
                .InstantiatePrefab(_gameConfig.SpaceshipViewPrefab, _levelCanvas.transform);
            
            Container
                .Bind<PlayerMovement>()
                .FromInstance(spaceship.GetComponent<PlayerMovement>())
                .AsSingle();
            
            Container
                .Bind<EnvironmentUnitSpawnService>()
                .AsSingle()
                .WithArguments(_levelCanvas.transform, spaceship.transform);

            Container
                .Bind<ProjectileSpawnService>()
                .AsSingle()
                .WithArguments(spaceship.transform);
            
            Container
                .Bind<SpaceshipModel>()
                .AsSingle()
                .WithArguments(_gameConfig.SpaceshipConfig);
            Container
                .Bind<SpaceshipViewModel>()
                .AsSingle();
            Container
                .Bind<SpaceshipView>()
                .FromInstance(spaceship.GetComponent<SpaceshipView>())
                .AsSingle();
            
            Container
                .Bind<AmmoModel>()
                .AsSingle()
                .WithArguments(_gameConfig.AmmoConfig);
            Container
                .Bind<AmmoViewModel>()
                .AsSingle();
            Container
                .Bind<AmmoView>()
                .FromInstance(_ammoView)
                .AsSingle();
            
            Container
                .Bind<ScoreModel>()
                .AsSingle();
            Container
                .Bind<ScoreViewModel>()
                .AsSingle();
            Container
                .Bind<ScoreView>()
                .FromInstance(_scoreView)
                .AsSingle();
        }
    }
}