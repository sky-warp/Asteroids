using System;

namespace _Project.Scripts.Firebase
{
    [Serializable]
    public class RemoteData
    {    
        public float SpaceshipSpeed;
        public int LaserCount;
        public float LaserCooldown;
        public float LaserSpeed;
        public float BulletSpeed;
        public float MinSpawnSpace;
        public float MaxSpawnSpace;
        public float BigAsteroidUnitSpeed;
        public int BigAsteroidUnitScore;
        public float SmallAsteroidUnitSpeed;
        public int SmallAsteroidUnitScore;
        public float UfoChaserUnitSpeed;
        public int UfoChaserUnitScore;
    }
}