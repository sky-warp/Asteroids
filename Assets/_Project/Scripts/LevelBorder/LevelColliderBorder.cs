using _Project.Scripts.Environment.EnvironmentUnitTypes;
using _Project.Scripts.Projectiles.ProjectileTypes;
using _Project.Scripts.Spaceship.View;
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
        public readonly Subject<AsteroidSmall> OnSmallAsteroidExit = new();

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Bullet bullet))
            {
                OnBulletExit?.OnNext(bullet);
            }

            if (other.TryGetComponent(out Laser laser))
            {
                OnLaserExit?.OnNext(laser);
            }

            if (other.TryGetComponent(out AsteroidBig asteroidBig))
            {
                OnBigAsteroidExit?.OnNext(other.gameObject.GetComponent<AsteroidBig>());
            }

            if (other.TryGetComponent(out AsteroidSmall asteroidSmall))
            {
                OnSmallAsteroidExit?.OnNext(other.gameObject.GetComponent<AsteroidSmall>());
            }

            if (other.TryGetComponent(out SpaceshipView playerMovement))
            {
                TeleportSpaceship(other);
            }
        }

        private void TeleportSpaceship(Collider2D playerCollider)
        {
            var spaceship = playerCollider.GetComponent<SpaceshipView>();
            if (spaceship == null) return;

            Vector2 playerPosition = playerCollider.transform.position;
            Vector2 colliderPosition = transform.position;
            Vector2 colliderSize = GetComponent<BoxCollider2D>().size;

            if (playerPosition.x < colliderPosition.x - colliderSize.x / 2)
            {
                spaceship.transform.position = new Vector2(colliderPosition.x + colliderSize.x / 2, playerPosition.y);
            }
            else if (playerPosition.x > colliderPosition.x + colliderSize.x / 2)
            {
                spaceship.transform.position = new Vector2(colliderPosition.x - colliderSize.x / 2, playerPosition.y);
            }
            else if (playerPosition.y < colliderPosition.y - colliderSize.y / 2)
            {
                spaceship.transform.position = new Vector2(playerPosition.x, colliderPosition.y + colliderSize.y / 2);
            }
            else if (playerPosition.y > colliderPosition.y + colliderSize.y / 2)
            {
                spaceship.transform.position = new Vector2(playerPosition.x, colliderPosition.y - colliderSize.y / 2);
            }
        }
    }
}