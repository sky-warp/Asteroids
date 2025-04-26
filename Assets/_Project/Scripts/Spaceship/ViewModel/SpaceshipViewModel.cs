using _Project.Scripts.GameStateServices;
using _Project.Scripts.Player;
using _Project.Scripts.Spaceship.Model;
using R3;
using UnityEngine;

namespace _Project.Scripts.Spaceship.ViewModel
{
    public class SpaceshipViewModel : Zenject.IInitializable
    {
        public readonly ReactiveProperty<float> SpaceshipSpeedView = new();
        public readonly ReactiveProperty<float> CoordinateXView = new();
        public readonly ReactiveProperty<float> CoordinateYView = new();
        public readonly ReactiveProperty<float> RotationAngleView = new();
        public readonly ReactiveProperty<bool> IsGameOver = new();
        public readonly ReactiveProperty<bool> IsGameResume = new();

        private SpaceshipModel _spaceshipModel;

        private DefaultGameStateService _defaultGameStateService;

        private DefaultGameStateService _defaultGameOverService;

        private PlayerMovement _playerMovement;

        private CompositeDisposable _disposable = new();

        public SpaceshipViewModel(SpaceshipModel spaceshipModel, DefaultGameStateService defaultGameStateService,
            PlayerMovement playerMovement)
        {
            _spaceshipModel = spaceshipModel;
            _defaultGameStateService = defaultGameStateService;
            _playerMovement = playerMovement;

            ResetStats();
        }

        public void Initialize()
        {
            _spaceshipModel.ShipSpeed
                .Subscribe(x => SpaceshipSpeedView.Value = x)
                .AddTo(_disposable);
            _spaceshipModel.CoordinateX
                .Subscribe(x => CoordinateXView.Value = x)
                .AddTo(_disposable);
            _spaceshipModel.CoordinateY
                .Subscribe(x => CoordinateYView.Value = x)
                .AddTo(_disposable);
            _spaceshipModel.RotationAngle
                .Subscribe(x => RotationAngleView.Value = x)
                .AddTo(_disposable);

            _defaultGameStateService.OnGameOver
                .Subscribe(_ => IsGameOver.Value = true)
                .AddTo(_disposable); 
            _defaultGameStateService.OnGameOver
                .Subscribe(_ => IsGameResume.Value = false)
                .AddTo(_disposable); 
            _defaultGameStateService.OnGameResume
                .Subscribe(_ => IsGameOver.Value = false)
                .AddTo(_disposable); 
            _defaultGameStateService.OnGameResume
                .Subscribe(_ => IsGameResume.Value = true)
                .AddTo(_disposable);

            _playerMovement.CurrentSpeed
                .Subscribe(currentSpeed =>
                    SpaceshipSpeedView.Value = Mathf.Clamp(currentSpeed, 0, currentSpeed))
                .AddTo(_disposable);
            _playerMovement.CurrentXPosition
                .Subscribe(currentX => CoordinateXView.Value = currentX)
                .AddTo(_disposable);
            _playerMovement.CurrentYPosition
                .Subscribe(currentY => CoordinateYView.Value = currentY)
                .AddTo(_disposable);
            _playerMovement.CurrentRotationAngle
                .Subscribe(currentRotation => RotationAngleView.Value = currentRotation)
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        private void ResetStats()
        {
            SpaceshipSpeedView.Value = _spaceshipModel.ShipSpeed.Value;
            CoordinateXView.Value = _spaceshipModel.CoordinateX.Value;
            CoordinateYView.Value = _spaceshipModel.CoordinateY.Value;
            RotationAngleView.Value = _spaceshipModel.RotationAngle.Value;
        }

        public void ApplyStats()
        {
            _spaceshipModel.ShipSpeed.Value = SpaceshipSpeedView.Value;
            CoordinateXView.Value = _spaceshipModel.CoordinateX.Value;
            CoordinateYView.Value = _spaceshipModel.CoordinateY.Value;
            RotationAngleView.Value = _spaceshipModel.RotationAngle.Value;
        }
    }
}