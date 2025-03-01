using _Project.Scripts.Configs.AmmoConfig;
using _Project.Scripts.Configs.Spaceship;
using _Project.Scripts.Projectile.Ammo.Model;
using _Project.Scripts.Projectile.Ammo.View;
using _Project.Scripts.Projectile.Ammo.ViewModel;
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

        private SpaceshipViewModel _spaceshipViewModel;
        private AmmoViewModel _ammoViewModel;
        
        private void Start()
        {
            SpaceshipModel spaceshipModel = new SpaceshipModel(_spaceshipConfig);
            _spaceshipViewModel = new SpaceshipViewModel(spaceshipModel);
            _spaceshipView.Init(_spaceshipViewModel);

            AmmoModel ammoModel = new AmmoModel(_ammoConfig);
            _ammoViewModel = new AmmoViewModel(ammoModel);
            _ammoView.Init(_ammoViewModel);
        }

        private void OnDestroy()
        {
            _spaceshipViewModel.DisposableSpaceshipViewModel?.Dispose();
            _ammoViewModel?.Disposable.Dispose();
        }
    }
}