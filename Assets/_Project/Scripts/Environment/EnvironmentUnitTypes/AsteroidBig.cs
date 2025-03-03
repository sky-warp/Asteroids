using _Project.Scripts.Projectiles.ProjectileTypes;
using R3;
using UnityEngine;

namespace _Project.Scripts.Environment.EnvironmentUnitTypes
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class AsteroidBig : EnvironmentObject
    {
        public readonly Subject<AsteroidBig> OnBigAsteroidHit = new();
        [field: SerializeField] public int SmallAsteroidsAmountAfterHit { get; private set; }

        public void MoveAsteroidBig(Vector2 direction)
        {
            Rigidbody2D.velocity = direction * Speed;
        }

        private new void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            
            if (other.TryGetComponent(out Bullet bullet) || other.TryGetComponent(out Laser laser))
            {
                OnBigAsteroidHit?.OnNext(gameObject.GetComponent<AsteroidBig>());
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out UfoChaser ufoChaser))
                Physics2D.IgnoreCollision(other.collider, gameObject.GetComponent<Collider2D>());
        }
    }
}