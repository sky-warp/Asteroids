using System;
using UnityEngine;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _playerSpeed;
        [SerializeField] private float _rotateSpeed;
        [Range(0, 1)] [SerializeField] private float _moveDecay;
        [SerializeField] private float _acceleration;

        private Rigidbody2D _rigidbody2D;
        private float _yInput;
        private float _xInput;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _yInput = Input.GetAxisRaw("Vertical");
            _xInput = Input.GetAxisRaw("Horizontal");
        }

        private void FixedUpdate()
        {
            if (_yInput > 0)
            {
                Vector2 direction = transform.up; 
                float increment = _yInput * _acceleration;
                Vector2 newVelocity = _rigidbody2D.velocity + direction * increment;
                newVelocity = Vector2.ClampMagnitude(newVelocity, _playerSpeed);
                _rigidbody2D.velocity = newVelocity;
            }
            else
            {
                _rigidbody2D.velocity *= _moveDecay;
            }

            if (MathF.Abs(_xInput) > 0)
            {
                _rigidbody2D.angularVelocity = _xInput * _rotateSpeed;
            }
            else
            {
                _rigidbody2D.angularVelocity *= _moveDecay;
            }
        }
    }
}