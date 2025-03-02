using _Project.Scripts.Configs.SpaceshipConfigs;
using R3;

namespace _Project.Scripts.Spaceship.Model
{
    public class SpaceshipModel
    {
        public readonly ReactiveProperty<float> ShipSpeed; 
        public readonly ReactiveProperty<float> CoordinateX  = new();
        public readonly ReactiveProperty<float> CoordinateY  = new();
        public readonly ReactiveProperty<float> RotationAngle  = new();
        
        public SpaceshipModel(SpaceshipConfig config)
        {
            ShipSpeed = new(config.Speed);
        }
    }
}