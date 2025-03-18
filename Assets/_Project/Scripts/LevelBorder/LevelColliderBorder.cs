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

        private SpaceshipView _spaceship;
        private BoxCollider2D _boxCollider;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _boxCollider = GetComponent<BoxCollider2D>();
            AdjustCollider();
        }

        public void Init(SpaceshipView spaceship)
        {
            _spaceship = spaceship;
        }

        private void AdjustCollider()
        {
            Vector2 bottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
            Vector2 topRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
            
            float width = topRight.x - bottomLeft.x;
            float height = topRight.y - bottomLeft.y;
            
            _boxCollider.size = new Vector2(width, height);
            
            _boxCollider.offset = Vector2.zero;
            transform.position = _camera.transform.position;
        }
        
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
        }
    }
}