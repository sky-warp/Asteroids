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
        [SerializeField] private Canvas _levelCanvas;
        
        public readonly Subject<Bullet> OnBulletExit = new();
        public readonly Subject<Laser> OnLaserExit = new();
        public readonly Subject<AsteroidBig> OnBigAsteroidExit = new();
        public readonly Subject<AsteroidSmall> OnSmallAsteroidExit = new();

        private SpaceshipView _spaceship;
        private RectTransform _canvasRect;
        private Vector2 _canvasSize;
        private Vector2 _canvasPosition;

        public void Init(SpaceshipView spaceship)
        {
            _spaceship = spaceship;
            
            _canvasRect = _levelCanvas.GetComponent<RectTransform>();
            _canvasSize = _canvasRect.sizeDelta * _levelCanvas.scaleFactor;
            _canvasPosition = _levelCanvas.transform.position;
        }
        
        private void Update()
        {
            CheckCanvasTeleport(_spaceship);
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

        private void CheckCanvasTeleport(SpaceshipView spaceship)
        {
            if (spaceship == null) return;
            
            Vector2 playerPosition = spaceship.transform.position;

            if (playerPosition.x < _canvasPosition.x - _canvasSize.x / 2 ||
                playerPosition.x > _canvasPosition.x + _canvasSize.x / 2 ||
                playerPosition.y < _canvasPosition.y - _canvasSize.y / 2 ||
                playerPosition.y > _canvasPosition.y + _canvasSize.y / 2)
            {
                TeleportShip(spaceship.transform, playerPosition, _canvasPosition, _canvasSize);
            }
        }

        private void TeleportShip(Transform spaceshipTransform, Vector2 playerPosition, Vector2 canvasPosition, Vector2 canvasSize)
        {
            Vector2 newPosition = playerPosition;
            
            if (playerPosition.x < canvasPosition.x - canvasSize.x / 2)
            {
                newPosition.x = canvasPosition.x + canvasSize.x / 2;
            }
            else if (playerPosition.x > canvasPosition.x + canvasSize.x / 2)
            {
                newPosition.x = canvasPosition.x - canvasSize.x / 2;
            }

            if (playerPosition.y < canvasPosition.y - canvasSize.y / 2)
            {
                newPosition.y = canvasPosition.y + canvasSize.y / 2;
            }
            else if (playerPosition.y > canvasPosition.y + canvasSize.y / 2)
            {
                newPosition.y = canvasPosition.y - canvasSize.y / 2;
            }
            
            spaceshipTransform.position = newPosition;
        }
    }
}