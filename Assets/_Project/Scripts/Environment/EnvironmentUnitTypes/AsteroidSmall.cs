using _Project.Scripts.Projectiles.ProjectileTypes;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Environment.EnvironmentUnitTypes
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class AsteroidSmall : EnvironmentObject
    {
        public readonly Subject<AsteroidSmall> OnSmallAsteroidHit = new();

        public void MoveSmallAsteroid(Vector2 position)
        {
            transform.position = position;
            
            Vector2 direction = Random.insideUnitCircle.normalized;
            
            Rigidbody2D.velocity = direction * Speed;
        }
        
        private new void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            
            if (other.TryGetComponent(out Bullet bullet) || other.TryGetComponent(out Laser laser))
            {
                OnSmallAsteroidHit?.OnNext(gameObject.GetComponentInParent<AsteroidSmall>());
            }
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out UfoChaser ufoChaser))
                Physics2D.IgnoreCollision(other.collider, gameObject.GetComponent<Collider2D>());
        }
    }
}