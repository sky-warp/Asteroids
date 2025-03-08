using _Project.Scripts.Spaceship.Model;
using R3;

namespace _Project.Scripts.Spaceship.ViewModel
{
    public class SpaceshipViewModel
    {
        public readonly ReactiveProperty<float> SpaceshipSpeedView = new();
        public readonly ReactiveProperty<float> CoordinateXView = new();
        public readonly ReactiveProperty<float> CoordinateYView = new();
        public readonly ReactiveProperty<float> RotationAngleView = new();

        private SpaceshipModel _spaceshipModel;
        private CompositeDisposable _disposable = new();

        public SpaceshipViewModel(SpaceshipModel spaceshipModel)
        {
            _spaceshipModel = spaceshipModel;

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

            ResetStats();
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