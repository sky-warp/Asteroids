using _Project.Scripts.Configs.SpaceshipConfigs;
using R3;

namespace _Project.Scripts.Spaceship.Model
{
    public class SpaceshipModel
    {
        public ReactiveProperty<float> ShipSpeed { get; private set; }
        public ReactiveProperty<float> CoordinateX { get; private set; } = new();
        public ReactiveProperty<float> CoordinateY { get; private set; } = new();
        public ReactiveProperty<float> RotationAngle { get; private set; } = new();
        
        public SpaceshipModel(SpaceshipConfig config)
        {
            ShipSpeed = new(config.Speed);
        }
    }
}