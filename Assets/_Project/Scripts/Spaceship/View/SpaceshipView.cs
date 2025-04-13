using _Project.Scripts.Infrastructure;
using _Project.Scripts.Player;
using _Project.Scripts.Spaceship.ViewModel;
using R3;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Spaceship.View
{
    public class SpaceshipView : MonoBehaviour
    {
        [SerializeField] private Sprite _shipSprite;

        [SerializeField] private TextMeshProUGUI _currentSpeedText;
        [SerializeField] private TextMeshProUGUI _coordinateXText;
        [SerializeField] private TextMeshProUGUI _coordinateYText;
        [SerializeField] private TextMeshProUGUI _rotationRotationText;

        private SpaceShipStats _statsParent;
        private PlayerMovement _playerMovement;
        private SpaceshipViewModel _spaceshipViewModel;

        public void Init(SpaceshipViewModel spaceshipViewModel, SpaceShipStats statsParent)
        {
            _statsParent = statsParent;
            _spaceshipViewModel = spaceshipViewModel;
            
            var currentSpeed = Instantiate(_currentSpeedText, _statsParent.SpaceShipStatsTransform);
            var currentX = Instantiate(_coordinateXText, _statsParent.SpaceShipStatsTransform);
            var currentY = Instantiate(_coordinateYText, _statsParent.SpaceShipStatsTransform);
            var currentAngle = Instantiate(_rotationRotationText, _statsParent.SpaceShipStatsTransform);

            _spaceshipViewModel.SpaceshipSpeedView
                .Subscribe(_ => _spaceshipViewModel.ApplyStats())
                .AddTo(this);
            _spaceshipViewModel.CoordinateXView
                .Subscribe(_ => _spaceshipViewModel.ApplyStats())
                .AddTo(this);
            _spaceshipViewModel.CoordinateYView
                .Subscribe(_ => _spaceshipViewModel.ApplyStats())
                .AddTo(this);
            _spaceshipViewModel.RotationAngleView
                .Subscribe(_ => _spaceshipViewModel.ApplyStats())
                .AddTo(this);

            _spaceshipViewModel.SpaceshipSpeedView
                .Subscribe(speed => (currentSpeed).text = $"SPEED: {speed:F2}")
                .AddTo(this);
            _spaceshipViewModel.CoordinateXView
                .Subscribe(x => currentX.text = $"X: {x:F2}")
                .AddTo(this);
            _spaceshipViewModel.CoordinateYView
                .Subscribe(y => currentY.text = $"Y: {y:F2}")
                .AddTo(this);
            _spaceshipViewModel.RotationAngleView
                .Subscribe(rotation => currentAngle.text = $"ROTATION: {rotation:F2}")
                .AddTo(this);
            
            _spaceshipViewModel.IsGameOver
                .Where(isGameOver => isGameOver)
                .Subscribe(_ => GameOver())
                .AddTo(this);
            _spaceshipViewModel.IsGameResume
                .Where(isGameResume => isGameResume)
                .Subscribe(_ => GameResume())
                .AddTo(this);
        }

        private void GameOver()
        {
            _statsParent.SpaceShipStatsTransform.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private void GameResume()
        {
            _statsParent.SpaceShipStatsTransform.gameObject.SetActive(true);
            gameObject.SetActive(true);
        }
    }
}