using R3;

namespace _Project.Scripts.Projectiles.Ammo.Model
{
    public class AmmoModel
    {
        public readonly ReactiveProperty<int> CurrentLaserAmmo = new();
        public readonly ReactiveProperty<float> LaserCooldown = new();
        public readonly ReactiveProperty<bool> IsEnoughLaserAmmo = new(true);
        
        public readonly int MaxAmmo;
        
        public AmmoModel(int maxAmmo, float laserCooldown)
        {
            CurrentLaserAmmo.Value = maxAmmo;
            LaserCooldown.Value = laserCooldown;

            MaxAmmo = CurrentLaserAmmo.Value;
        }
    }
}