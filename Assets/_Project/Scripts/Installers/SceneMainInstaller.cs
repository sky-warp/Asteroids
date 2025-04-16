using _Project.Scripts.DOTweenAnimations;
using _Project.Scripts.Environment.Units;
using _Project.Scripts.Factories;
using _Project.Scripts.Firebase;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.InputService;
using _Project.Scripts.LevelBorder;
using _Project.Scripts.LocalAssetLoaders;
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
using _Project.Scripts.UnityAds.Controller;
using _Project.Scripts.UnityAds.View;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class SceneMainInstaller : MonoInstaller
    {
        [SerializeField] private Transform _spaceshipStatsParent;

        [SerializeField] private AmmoView _ammoView;
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private AdsView _adsView;

        [SerializeField] private LevelColliderBorder _levelColliderBorder;

        [SerializeField] public EndGameWindowAppearAnimation _endGameAnimation;

        [Inject] private readonly MainLevelResources _mainLevelResources;

        [Inject] private readonly RemoteData _remoteData;

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
                .AsSingle()
                .WithArguments(_remoteData.MinSpawnSpace, _remoteData.MaxSpawnSpace);

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
                .InstantiatePrefabForComponent<SpaceshipView>(_mainLevelResources.Spaceship);

            Container
                .Bind<PlayerMovement>()
                .FromInstance(spaceship.GetComponent<PlayerMovement>())
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<EnvironmentUnitSpawnService>()
                .AsSingle()
                .WithArguments(
                    spaceship.transform,
                    new EnvironmentUnitFactory<AsteroidBig>(_mainLevelResources.AsteroidBig,
                        _remoteData.BigAsteroidUnitSpeed, _remoteData.BigAsteroidUnitScore),
                    new EnvironmentUnitFactory<AsteroidSmall>(_mainLevelResources.AsteroidSmall,
                        _remoteData.SmallAsteroidUnitSpeed, _remoteData.SmallAsteroidUnitScore),
                    new EnvironmentUnitFactory<UfoChaser>(_mainLevelResources.UfoChaser, _remoteData.UfoChaserUnitSpeed,
                        _remoteData.UfoChaserUnitScore)
                );

            Container
                .BindInterfacesAndSelfTo<ProjectileSpawnService>()
                .AsSingle()
                .WithArguments(
                    spaceship.transform,
                    new ProjectileFactory<Bullet>(_mainLevelResources.Bullet, _remoteData.BulletSpeed),
                    new ProjectileFactory<Laser>(_mainLevelResources.Laser, _remoteData.LaserSpeed)
                );

            Container
                .Bind<SpaceShipStats>()
                .FromMethod(_ => new SpaceShipStats(_spaceshipStatsParent))
                .AsSingle();

            Container
                .Bind<SpaceshipModel>()
                .AsSingle()
                .WithArguments(_remoteData.SpaceshipSpeed);
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
                .WithArguments(_remoteData.LaserCount, _remoteData.LaserCooldown);
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

            Container
                .BindInterfacesAndSelfTo<AdsController>()
                .AsSingle();
            Container
                .Bind<AdsView>()
                .FromInstance(_adsView)
                .AsSingle();
        }
    }
}