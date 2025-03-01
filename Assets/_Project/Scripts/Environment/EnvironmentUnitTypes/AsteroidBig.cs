using _Project.Scripts.Configs.EnvironmentConfigs;
using _Project.Scripts.Projectiles.ProjectileTypes;
using R3;
using UnityEngine;

namespace _Project.Scripts.Environment.EnvironmentUnitTypes
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class AsteroidBig : MonoBehaviour
    {
        public readonly Subject<AsteroidBig> OnBigAsteroidHit = new();
        
        [SerializeField] private EnvironmentUnitConfig _environmentUnitConfig;
        
        private float _speed;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _speed = _environmentUnitConfig.UnitSpeed;
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void MoveAsteroidBig(Vector2 direction)
        {
            _rigidbody2D.velocity = direction * _speed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Bullet>() || other.gameObject.GetComponent<Laser>())
            {
                OnBigAsteroidHit?.OnNext(gameObject.GetComponent<AsteroidBig>());
            }
        }
    }
}