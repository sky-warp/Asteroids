using _Project.Scripts.Configs.Spaceship;
using _Project.Scripts.Spaceship.Model;
using _Project.Scripts.Spaceship.View;
using _Project.Scripts.Spaceship.ViewModel;
using UnityEngine;

namespace _Project.Scripts.Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private SpaceshipConfig _spaceshipConfig;
        
        [SerializeField] private SpaceshipView _spaceshipView;

        private void Start()
        {
            SpaceshipModel spaceshipModel = new SpaceshipModel(_spaceshipConfig);
            SpaceshipViewModel spaceshipViewModel = new SpaceshipViewModel(spaceshipModel);
            _spaceshipView.Init(spaceshipViewModel);
        }
    }
}