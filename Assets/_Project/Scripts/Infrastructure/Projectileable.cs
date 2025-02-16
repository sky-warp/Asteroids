using _Project.Scripts.Configs;
using UnityEngine;

namespace _Project.Scripts.Infrastructure
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectileable : MonoBehaviour
    {
        [field: SerializeField] public ProjectileConfig Config { get; private set; }
        
        private Rigidbody2D _rigidbody2d;
        private Transform _sourceObjectPosition;

        private void Awake()
        {
            _sourceObjectPosition = GetComponentInParent<Transform>();
            _rigidbody2d = GetComponent<Rigidbody2D>();
        }

        public void MoveProjectile()
        {
            Vector2 direction = _sourceObjectPosition.transform.up;
            _rigidbody2d.velocity = direction * Config.Speed;
            transform.position = _sourceObjectPosition.position;
        }
    }
}