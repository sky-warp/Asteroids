using System;
using _Project.Scripts.Projectiles.ProjectileTypes;
using R3;
using UnityEngine;

namespace _Project.Scripts.Environment.Units
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class UfoChaser : EnvironmentObject
    {
        public readonly Subject<UfoChaser> OnUfoHit = new();
        public readonly Subject<Unit> OnUfoDeath = new();
        public readonly ReactiveProperty<Vector2> TargetPosition = new();

        private IDisposable _moveTowards;
        
        public void MoveTowardsTarget()
        {
            _moveTowards = TargetPosition
                .Subscribe(playerPosition =>
                {
                    Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;
                    Rigidbody2D.velocity = direction * Speed;
                    
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
                    Rigidbody2D.rotation = angle;
                })
                .AddTo(Disposable);
        }

        public void StopChasing()
        {
            _moveTowards?.Dispose();
            Rigidbody2D.velocity = Vector2.zero;
        }
        
        private new void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            
            if (other.TryGetComponent(out Bullet bullet) || other.TryGetComponent(out Laser laser))
            {
                OnUfoHit?.OnNext(this);
                OnUfoDeath?.OnNext(Unit.Default);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out AsteroidSmall asteroidSmall) ||
                other.gameObject.TryGetComponent(out AsteroidBig asteroidBig))
                Physics2D.IgnoreCollision(other.collider, gameObject.GetComponent<Collider2D>());
        }
    }
}