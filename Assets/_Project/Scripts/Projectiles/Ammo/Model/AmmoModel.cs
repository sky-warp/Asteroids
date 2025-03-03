using _Project.Scripts.Configs.AmmoConfigs;
using R3;

namespace _Project.Scripts.Projectiles.Ammo.Model
{
    public class AmmoModel
    {
        public readonly ReactiveProperty<int> CurrentLaserAmmo = new();
        public readonly ReactiveProperty<float> LaserCooldown = new();
        public readonly ReactiveProperty<bool> IsEnoughLaserAmmo = new(true);
        
        public readonly int MaxAmmo;
        
        public AmmoModel(AmmoConfig config)
        {
            CurrentLaserAmmo.Value = config.LaserCount;
            LaserCooldown.Value = config.LaserCooldown;

            MaxAmmo = CurrentLaserAmmo.Value;
        }
    }
}