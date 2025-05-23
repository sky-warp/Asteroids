using System;
using _Project.Scripts.InputService;
using R3;
using UnityEngine;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        public readonly ReactiveProperty<float> CurrentSpeed = new();
        public readonly ReactiveProperty<float> CurrentXPosition = new();
        public readonly ReactiveProperty<float> CurrentYPosition = new();
        public readonly ReactiveProperty<float> CurrentRotationAngle = new();

        [SerializeField] private float _rotateSpeed;
        [Range(0, 1)] [SerializeField] private float _moveDecay;
        [SerializeField] private float _acceleration;

        private Rigidbody2D _rigidbody2D;
        private float _yInput;
        private float _xInput;
        private float _playerSpeed;
        private float _baseSpeed;
        private float _baseRotationSpeed;
        private IInputable _inputManager;

        public void Init(float speed, IInputable inputManager)
        {
            _playerSpeed = speed;
            _baseSpeed = _playerSpeed;
            _baseRotationSpeed = _rotateSpeed;
            _inputManager = inputManager;
        }

        public void GameOver()
        {
            _playerSpeed = 0;
            _rotateSpeed = 0;
        }

        public void GameResume()
        {
            _playerSpeed = _baseSpeed;
            _rotateSpeed = _baseRotationSpeed;
        }

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_inputManager != null)
            {
                _yInput = _inputManager.GetAxisVertical();
                _xInput = _inputManager.GetAxisHorizontal();
            }

            CurrentRotationAngle.Value = transform.eulerAngles.z;

            CurrentXPosition.Value = transform.position.x;
            CurrentYPosition.Value = transform.position.y;
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

                CurrentSpeed.Value = _rigidbody2D.velocity.y;
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

            CurrentSpeed.Value = _rigidbody2D.velocity.magnitude;
        }
    }
}