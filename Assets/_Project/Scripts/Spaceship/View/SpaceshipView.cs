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
        [SerializeField] private TextMeshProUGUI _rotationAngleText;

        private PlayerMovement _playerMovement;
        
        private SpaceshipViewModel _spaceshipViewModel;
        
        public void Init(SpaceshipViewModel spaceshipViewModel)
        {
            _spaceshipViewModel = spaceshipViewModel;
            
            _playerMovement = GetComponent<PlayerMovement>();
            _playerMovement.Init(_spaceshipViewModel.SpaceshipSpeedView.Value);
            //ViewModel subs
            _playerMovement.CurrentSpeed
                .Subscribe(currentSpeed => _spaceshipViewModel.SpaceshipSpeedView.Value = Mathf.Clamp(currentSpeed, 0, currentSpeed))
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
            
            //ApplyStats
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
            
            //View subs
            _spaceshipViewModel.SpaceshipSpeedView
                .Subscribe(speed => _currentSpeedText.text = $"SPEED: {speed:F2}")
                .AddTo(this);
            _spaceshipViewModel.CoordinateXView
                .Subscribe(x => _coordinateXText.text = $"X: {x:F2}")
                .AddTo(this);
            _spaceshipViewModel.CoordinateYView
                .Subscribe(y => _coordinateYText.text = $"Y: {y:F2}")
                .AddTo(this);
            _spaceshipViewModel.RotationAngleView
                .Subscribe(rotation => _rotationAngleText.text = $"ROTATION: {rotation:F2}")
                .AddTo(this);
            
            CreateShip();
        }
        
        private void CreateShip()
        {
            _shipImage.sprite = _shipSprite;
        }

        private void OnDestroy()
        {
            _spaceshipViewModel.DisposableSpaceshipViewModel?.Dispose();
        }
    }
}