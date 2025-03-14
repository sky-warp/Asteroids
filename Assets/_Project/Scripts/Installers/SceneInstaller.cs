using _Project.Scripts.Configs.GameConfigs;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LevelBorder;
using _Project.Scripts.Projectiles.Ammo.View;
using _Project.Scripts.Score.View;
using _Project.Scripts.Spaceship.View;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _gameConfig;

        [SerializeField] private Canvas _levelCanvas;

        [SerializeField] private Transform _spaceshipStatsParent;

        [SerializeField] private AmmoView _ammoView;
        [SerializeField] private ScoreView _scoreView;

        [SerializeField] private Transform[] _spawnPoints;

        [SerializeField] private LevelColliderBorder _levelColliderBorder;

        public override void InstallBindings()
        {
            Container
                .BindInterfacesTo<EntryPoint>()
                .AsSingle();

            var spaceship = Container
                .InstantiatePrefab(_gameConfig.SpaceshipViewPrefab, _levelCanvas.transform);
            
            Container
                .Bind<SpaceshipView>()
                .FromInstance(spaceship.GetComponent<SpaceshipView>())
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
                .Bind<AmmoView>()
                .FromInstance(_ammoView)
                .AsSingle();

            Container
                .Bind<ScoreView>()
                .FromInstance(_scoreView)
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
                .Bind<CoroutineManager.CoroutineManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle();
        }
    }
}