using _Project.Scripts.Configs.AmmoConfigs;
using _Project.Scripts.Configs.SpaceshipConfigs;
using _Project.Scripts.Projectiles.Ammo.Model;
using _Project.Scripts.Projectiles.Ammo.View;
using _Project.Scripts.Projectiles.Ammo.ViewModel;
using _Project.Scripts.Score.Model;
using _Project.Scripts.Score.View;
using _Project.Scripts.Score.ViewModel;
using _Project.Scripts.Spaceship.Model;
using _Project.Scripts.Spaceship.View;
using _Project.Scripts.Spaceship.ViewModel;
using UnityEngine;

namespace _Project.Scripts.Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private SpaceshipConfig _spaceshipConfig;
        [SerializeField] private AmmoConfig _ammoConfig;
        
        [SerializeField] private SpaceshipView _spaceshipView;
        [SerializeField] private AmmoView _ammoView;
        [SerializeField] private ScoreView _scoreView;

        private SpaceshipViewModel _spaceshipViewModel;
        private AmmoViewModel _ammoViewModel;
        private ScoreViewModel _scoreViewModel;
        
        private void Awake()
        {
            SpaceshipModel spaceshipModel = new SpaceshipModel(_spaceshipConfig);
            _spaceshipViewModel = new SpaceshipViewModel(spaceshipModel);
            _spaceshipView.Init(_spaceshipViewModel);

            AmmoModel ammoModel = new AmmoModel(_ammoConfig);
            _ammoViewModel = new AmmoViewModel(ammoModel);
            _ammoView.Init(_ammoViewModel);

            ScoreModel scoreModel = new();
            _scoreViewModel = new ScoreViewModel(scoreModel);
            _scoreView.Init(_scoreViewModel);
        }

        private void OnDestroy()
        {
            _spaceshipViewModel.DisposableSpaceshipViewModel?.Dispose();
            _ammoViewModel?.Disposable.Dispose();
            _scoreViewModel?.Disposable.Dispose();
        }
    }
}