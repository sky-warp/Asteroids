using _Project.Scripts.Configs.GameConfigs;
using _Project.Scripts.DOTweenAnimations;
using _Project.Scripts.Environment.Units;
using _Project.Scripts.Factories;
using _Project.Scripts.Infrastructure;
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
using Zenject;

namespace _Project.Scripts.Installers
{
    public class SceneMainInstaller : MonoInstaller
    {
        [SerializeField] private Transform _spaceshipStatsParent;

        [SerializeField] private AmmoView _ammoView;
        [SerializeField] private ScoreView _scoreView;

        [SerializeField] private LevelColliderBorder _levelColliderBorder;

        [SerializeField] public EndGameWindowAppearAnimation _endGameAnimation;
        
        [Inject] private readonly GameConfig _gameConfig;
        
        public override void InstallBindings()
        {
            Container
                .Bind<Camera>()
                .FromInstance(Camera.main)
                .AsSingle();

            Container
                .BindInterfacesTo<EntryPoint>()
                .AsSingle();

            Container
                .Bind<SpawnRandomizer>()
                .AsSingle();

            Container
                .Bind<LevelColliderBorder>()
                .FromInstance(_levelColliderBorder)
                .AsSingle();

            Container
                .Bind<IInputable>()
                .To<InputManager>()
                .AsSingle();

            Container
                .Bind<EndGameWindowAppearAnimation>()
                .FromInstance(_endGameAnimation)
                .AsSingle();
            
            var spaceship = Container
                .InstantiatePrefabForComponent<SpaceshipView>(_gameConfig.SpaceshipViewPrefab);

            Container
                .Bind<PlayerMovement>()
                .FromInstance(spaceship.GetComponent<PlayerMovement>())
                .AsSingle();
            
            Container
                .BindInterfacesAndSelfTo<EnvironmentUnitSpawnService>()
                .AsSingle()
                .WithArguments(
                    spaceship.transform, 
                    new MonoFactory<AsteroidBig>(_gameConfig.AsteroidBigPrefab),
                    new MonoFactory<AsteroidSmall>(_gameConfig.AsteroidSmallPrefab),
                    new MonoFactory<UfoChaser>(_gameConfig.UfoChaserPrefab)
                    );

            Container
                .BindInterfacesAndSelfTo<ProjectileSpawnService>()
                .AsSingle()
                .WithArguments(
                    spaceship.transform,
                    new MonoFactory<Bullet>(_gameConfig.BulletPrefab),
                    new MonoFactory<Laser>(_gameConfig.LaserPrefab)
                    );

            Container
                .Bind<SpaceShipStats>()
                .FromMethod(_ => new SpaceShipStats(_spaceshipStatsParent))
                .AsSingle();

            Container
                .Bind<SpaceshipModel>()
                .AsSingle()
                .WithArguments(_gameConfig.SpaceshipConfig);
            Container
                .BindInterfacesAndSelfTo<SpaceshipViewModel>()
                .AsSingle();
            Container
                .Bind<SpaceshipView>()
                .FromInstance(spaceship)
                .AsSingle();

            Container
                .Bind<AmmoModel>()
                .AsSingle()
                .WithArguments(_gameConfig.AmmoConfig);
            Container
                .BindInterfacesAndSelfTo<AmmoViewModel>()
                .AsSingle();
            Container
                .Bind<AmmoView>()
                .FromInstance(_ammoView)
                .AsSingle();

            Container
                .Bind<ScoreModel>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<ScoreViewModel>()
                .AsSingle();
            Container
                .Bind<ScoreView>()
                .FromInstance(_scoreView)
                .AsSingle();
        }
    }
}