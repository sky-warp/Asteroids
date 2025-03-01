using _Project.Scripts.Configs.EnvironmentConfigs;
using UnityEngine;

namespace _Project.Scripts.Environment.EnvironmentUnitTypes
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class AsteroidSmall : MonoBehaviour
    {
        [SerializeField] private EnvironmentUnitConfig _environmentUnitConfig;

        private float _speed;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _speed = _environmentUnitConfig.UnitSpeed;
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void MoveSmallAsteroid(Vector2 position)
        {
            transform.position = position;
            
            Vector2 direction = Random.insideUnitCircle.normalized;
            
            _rigidbody2D.velocity = direction * _speed;
        }
    }
}