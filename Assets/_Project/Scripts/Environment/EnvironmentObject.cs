using System;
using _Project.Scripts.Configs.EnvironmentConfigs;
using _Project.Scripts.Projectiles.ProjectileTypes;
using _Project.Scripts.Spaceship.View;
using R3;
using UnityEngine;

namespace _Project.Scripts.Environment
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class EnvironmentObject : MonoBehaviour
    {
        public readonly Subject<Unit> OnSpaceshipTouched = new();
        public readonly Subject<Vector3> UnitPositionWhenHit = new();
        public int Score { get; private set; }
        protected float Speed { get; private set; }
        private float _initialSpeed;
        protected Rigidbody2D Rigidbody2D { get; private set; }
        public CompositeDisposable Disposable { get; private set; } = new();
        
        private void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void SetSpeed(float speed)
        {
            Speed = speed;
            _initialSpeed = Speed;
        }

        public void SetScore(int score)
        {
            Score = score;
        }
        
        public void AddSubscription(IDisposable subscription)
        {
            Disposable.Add(subscription);
        }
        
        public void ResetSubscription()
        {
            Disposable?.Dispose();
            Disposable = new();
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out SpaceshipView movableShip))
            {
                OnSpaceshipTouched?.OnNext(Unit.Default);
            }
            
            if (other.TryGetComponent(out Bullet big) || other.TryGetComponent(out Laser small))
            {
                UnitPositionWhenHit?.OnNext(transform.position);
            }
        }

        private void OnDisable()
        {
            Speed = _initialSpeed;
        }

        private void OnDestroy()
        {
            Disposable?.Dispose();
        }
    }
}