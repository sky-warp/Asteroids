using System;
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
        
        public int Score { get; private set; }
        
        [SerializeField] private EnvironmentUnitConfig _environmentUnitConfig;
        [field: SerializeField] public int SmallAsteroidsAmountAfterHit { get; private set; }
        
        private float _speed;
        private Rigidbody2D _rigidbody2D;
        private CompositeDisposable _disposable = new();

        private void Awake()
        {
            _speed = _environmentUnitConfig.UnitSpeed;
            Score = _environmentUnitConfig.UnitScore;
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void AddSubscription(IDisposable subscription)
        {
            _disposable.Add(subscription);
        }

        public void ResetSubscription()
        {
            _disposable.Dispose();
            _disposable = new();
        }

        public void MoveAsteroidBig(Vector2 direction)
        {
            _rigidbody2D.velocity = direction * _speed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Bullet bullet) || other.TryGetComponent(out Laser laser))
            {
                OnBigAsteroidHit?.OnNext(gameObject.GetComponent<AsteroidBig>());
            }
        }
    }
}