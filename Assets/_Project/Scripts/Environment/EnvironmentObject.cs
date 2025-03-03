using System;
using _Project.Scripts.Configs.EnvironmentConfigs;
using _Project.Scripts.Spaceship.View;
using R3;
using UnityEngine;

namespace _Project.Scripts.Environment
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class EnvironmentObject : MonoBehaviour
    {
        public Subject<Unit> OnSpaceshipTouched = new();
        public int Score { get; private set; }
        public float Speed { get; private set; }
        public Rigidbody2D Rigidbody2D { get; private set; }
        public CompositeDisposable Disposable { get; private set; } = new();
        
        [SerializeField] private EnvironmentUnitConfig _environmentUnitConfig;

        private void Awake()
        {
            Score = _environmentUnitConfig.UnitScore;
            Speed = _environmentUnitConfig.UnitSpeed;
            Rigidbody2D = GetComponent<Rigidbody2D>();
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
            if (other.gameObject.TryGetComponent(out SpaceshipView movableShip))
            {
                OnSpaceshipTouched?.OnNext(Unit.Default);
            }
        }

        private void OnDestroy()
        {
            Disposable?.Dispose();
        }
    }
}