using _Project.Scripts.Configs.EnvironmentConfigs;
using _Project.Scripts.Projectiles.ProjectileTypes;
using R3;
using UnityEngine;

namespace _Project.Scripts.Environment.EnvironmentUnitTypes
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class UfoChaser : MonoBehaviour
    {
        public readonly Subject<UfoChaser> OnUfoHit = new();
        public readonly ReactiveProperty<Vector2> TargetPosition = new();

        public int Score { get; private set; }

        public CompositeDisposable Disposable { get; private set; } = new();
        
        [SerializeField] private EnvironmentUnitConfig _environmentUnitConfig;

        private float _speed;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _speed = _environmentUnitConfig.UnitSpeed;
            Score = _environmentUnitConfig.UnitScore;
            _rigidbody2D = GetComponent<Rigidbody2D>();

        }

        public void MoveTowardsTarget()
        {
            TargetPosition
                .Subscribe(playerPosition =>
                {
                    Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;
                    _rigidbody2D.velocity = direction * _speed;
                    
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
                    _rigidbody2D.rotation = angle;
                })
                .AddTo(Disposable);
        }

        public void ResetSubscription()
        {
            Disposable?.Dispose();
            Disposable = new();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Bullet bullet) || other.TryGetComponent(out Laser laser))
            {
                OnUfoHit?.OnNext(this);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out AsteroidSmall asteroidSmall) ||
                other.gameObject.TryGetComponent(out AsteroidBig asteroidBig))
                Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
        }
    }
}