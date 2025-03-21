using _Project.Scripts.Environment.Units;
using _Project.Scripts.Projectiles.ProjectileTypes;
using _Project.Scripts.Spaceship.View;
using Cinemachine;
using UnityEngine;
using R3;
using Zenject;

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

        [Inject] 
        private void Construct(Camera camera)
        {
            _camera = camera;
        }
        
        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            AdjustCollider();
        }

        private void Update()
        {
            if (_spaceship != null)
            {
                AdjustSpaceship();
            }
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

        private void AdjustSpaceship()
        {
            Vector3 viewportPosition = _camera.WorldToViewportPoint(_spaceship.transform.position);

            if (viewportPosition.x > 1 || viewportPosition.x < 0 || viewportPosition.y > 1 || viewportPosition.y < 0)
            {
                Vector3 newPosition = _spaceship.transform.position;

                if (viewportPosition.x > 1)
                {
                    newPosition.x = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane)).x;
                }
                else if (viewportPosition.x < 0)
                {
                    newPosition.x = _camera.ViewportToWorldPoint(new Vector3(1, 0, _camera.nearClipPlane)).x;
                }

                if (viewportPosition.y > 1)
                {
                    newPosition.y = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane)).y;
                }
                else if (viewportPosition.y < 0)
                {
                    newPosition.y = _camera.ViewportToWorldPoint(new Vector3(0, 1, _camera.nearClipPlane)).y;
                }

                _spaceship.transform.position = newPosition;
            }
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