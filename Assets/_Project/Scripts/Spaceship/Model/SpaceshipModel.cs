using R3;

namespace _Project.Scripts.Spaceship.Model
{
    public class SpaceshipModel
    {
        public readonly ReactiveProperty<float> ShipSpeed = new(); 
        public readonly ReactiveProperty<float> CoordinateX  = new();
        public readonly ReactiveProperty<float> CoordinateY  = new();
        public readonly ReactiveProperty<float> RotationAngle  = new();
        
        public SpaceshipModel(float speed)
        {
            ShipSpeed.Value = speed;
        }
    }
}