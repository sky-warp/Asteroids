using _Project.Scripts.Environment.EnvironmentUnitTypes;
using _Project.Scripts.Projectiles.ProjectileTypes;
using UnityEngine;
using R3;

namespace _Project.Scripts.LevelBorder
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelColliderBorder : MonoBehaviour
    {
        public readonly Subject<Bullet> OnBulletExit = new();
        public readonly Subject<Laser> OnLaserExit = new();
        public readonly Subject<AsteroidBig> OnBigAsteroidExit = new();

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Projectiles.ProjectileTypes.Projectile>())
            {
                if (other.GetComponent<Bullet>())
                {
                    var projectile = other.GetComponent<Bullet>();
                    OnBulletExit?.OnNext(projectile);
                }

                if (other.GetComponent<Laser>())
                {
                    var projectile = other.GetComponent<Laser>();
                    OnLaserExit?.OnNext(projectile);
                }
            }

            if (other.gameObject.GetComponent<AsteroidBig>())
            {
                OnBigAsteroidExit?.OnNext(other.gameObject.GetComponent<AsteroidBig>());
            }
            if (other.gameObject.GetComponent<AsteroidSmall>())
            {
                Destroy(other.gameObject);
            }
        }
    }
}