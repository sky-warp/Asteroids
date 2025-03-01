using _Project.Scripts.Projectiles;
using UnityEngine;
using R3;

namespace _Project.Scripts.LevelBorder
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelColliderBorder : MonoBehaviour
    {
        public readonly Subject<Bullet> OnBulletExit = new();
        public readonly Subject<Laser> OnLaserExit = new();

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Projectiles.Projectile>())
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
        }
    }
}