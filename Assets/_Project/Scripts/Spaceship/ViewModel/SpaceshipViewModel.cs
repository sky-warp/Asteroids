using _Project.Scripts.Spaceship.Model;
using R3;

namespace _Project.Scripts.Spaceship.ViewModel
{
    public class SpaceshipViewModel
    {
        public ReactiveProperty<float> SpaceshipSpeedView { get; private set; } = new();
        public ReactiveProperty<float> CoordinateXView { get; private set; } = new();
        public ReactiveProperty<float> CoordinateYView { get; private set; } = new();
        public ReactiveProperty<float> RotationAngleView { get; private set; } = new();
        public CompositeDisposable DisposableSpaceshipViewModel { get; private set; } = new();

        private SpaceshipModel _spaceshipModel;

        public SpaceshipViewModel(SpaceshipModel spaceshipModel)
        {
            _spaceshipModel = spaceshipModel;

            _spaceshipModel.ShipSpeed
                .Subscribe(x => SpaceshipSpeedView.Value = x)
                .AddTo(DisposableSpaceshipViewModel);

            _spaceshipModel.CoordinateX
                .Subscribe(x => CoordinateXView.Value = x)
                .AddTo(DisposableSpaceshipViewModel);

            _spaceshipModel.CoordinateY
                .Subscribe(x => CoordinateYView.Value = x)
                .AddTo(DisposableSpaceshipViewModel);

            _spaceshipModel.RotationAngle
                .Subscribe(x => RotationAngleView.Value = x)
                .AddTo(DisposableSpaceshipViewModel);

            ResetStats();
        }

        public void ResetStats()
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