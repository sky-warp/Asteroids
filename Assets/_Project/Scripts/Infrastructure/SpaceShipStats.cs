using UnityEngine;

namespace _Project.Scripts.Infrastructure
{
    public class SpaceShipStats
    {
        public Transform SpaceShipStatsTransform { get; }

        public SpaceShipStats(Transform spaceShipStatsTransform)
        {
            SpaceShipStatsTransform = spaceShipStatsTransform;
        }
    }
}