using _Project.Scripts.Configs.ProjectilesConfigs;
using _Project.Scripts.Environment.Units;
using R3;
using UnityEngine;

namespace _Project.Scripts.Projectiles.ProjectileTypes
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class Projectile : MonoBehaviour
    {
        [field: SerializeField] public ProjectileConfig Config { get; private set; }

        public readonly Subject<Projectile> OnUnitHit = new();
        
        private Rigidbody2D _rigidbody2D;
        private Transform _sourceObjectPosition;

        private float _initialSpeed;
        private float _speed;
        
        private void Awake()
        {
            _sourceObjectPosition = GetComponentInParent<Transform>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            _speed = Config.Speed;
            _initialSpeed = _speed;
        }

        public void MoveProjectile()
        {
            Vector2 direction = _sourceObjectPosition.transform.up;
            _rigidbody2D.velocity = direction * _speed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out AsteroidBig big) 
                || other.TryGetComponent(out AsteroidSmall small) 
                || other.TryGetComponent(out UfoChaser ufo))
            {
                OnUnitHit?.OnNext(gameObject.GetComponent<Projectile>());
            }
        }

        private void OnDisable()
        {
            _speed = _initialSpeed;
        }
    }
}