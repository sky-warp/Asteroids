using _Project.Scripts.Configs.EnvironmentConfigs;
using _Project.Scripts.Projectiles.ProjectileTypes;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Environment.EnvironmentUnitTypes
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class AsteroidSmall : MonoBehaviour
    {
        public int Score { get; private set; }
        
        [SerializeField] private EnvironmentUnitConfig _environmentUnitConfig;

        private float _speed;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _speed = _environmentUnitConfig.UnitSpeed;
            Score = _environmentUnitConfig.UnitScore;
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void MoveSmallAsteroid(Vector2 position)
        {
            transform.position = position;
            
            Vector2 direction = Random.insideUnitCircle.normalized;
            
            _rigidbody2D.velocity = direction * _speed;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Bullet bullet) || other.TryGetComponent(out Laser laser))
            {
                Destroy(gameObject);
            }
        }
    }
}