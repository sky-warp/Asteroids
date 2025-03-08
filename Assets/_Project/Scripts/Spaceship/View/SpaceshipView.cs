using _Project.Scripts.Player;
using _Project.Scripts.Spaceship.ViewModel;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Spaceship.View
{
    public class SpaceshipView : MonoBehaviour
    {
        [SerializeField] private Image _shipImage;
        [SerializeField] private Sprite _shipSprite;

        [SerializeField] private TextMeshProUGUI _currentSpeedText;
        [SerializeField] private TextMeshProUGUI _coordinateXText;
        [SerializeField] private TextMeshProUGUI _coordinateYText;
        [SerializeField] private TextMeshProUGUI _rotationRotationText;

        private Transform _statsParent;
        private PlayerMovement _playerMovement;
        private SpaceshipViewModel _spaceshipViewModel;

        public void Init(SpaceshipViewModel spaceshipViewModel, Transform statsParent,
            PauseGameService.PauseGameService pauseService)
        {
            pauseService.OnPause
                .Subscribe(_ => GameOver())
                .AddTo(this);
            
            CreateShip();

            _statsParent = statsParent;
            
            var currentSpeed = Instantiate(_currentSpeedText, _statsParent);
            var currentX = Instantiate(_coordinateXText, _statsParent);
            var currentY = Instantiate(_coordinateYText, _statsParent);
            var currentAngle = Instantiate(_rotationRotationText, _statsParent);

            _spaceshipViewModel = spaceshipViewModel;

            _playerMovement = GetComponent<PlayerMovement>();
            _playerMovement.Init(_spaceshipViewModel.SpaceshipSpeedView.Value);

            _playerMovement.CurrentSpeed
                .Subscribe(currentSpeed =>
                    _spaceshipViewModel.SpaceshipSpeedView.Value = Mathf.Clamp(currentSpeed, 0, currentSpeed))
                .AddTo(this);
            _playerMovement.CurrentXPosition
                .Subscribe(currentX => _spaceshipViewModel.CoordinateXView.Value = currentX)
                .AddTo(this);
            _playerMovement.CurrentYPosition
                .Subscribe(currentY => _spaceshipViewModel.CoordinateYView.Value = currentY)
                .AddTo(this);
            _playerMovement.CurrentRotationAngle
                .Subscribe(currentRotation => _spaceshipViewModel.RotationAngleView.Value = currentRotation)
                .AddTo(this);

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
        }

        private void GameOver()
        {
            _statsParent.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private void CreateShip()
        {
            _shipImage.sprite = _shipSprite;
        }
    }
}