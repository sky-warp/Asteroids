using System;
using R3;
using UnityEngine;

namespace _Project.Scripts.InputService
{
    public class InputManager : IInputable
    {
        public ReactiveProperty<bool> IsAvailable { get; }
        public event Action OnLeftClick;
        public event Action OnRightClick;
        
        private readonly CompositeDisposable _disposable = new();
        
        public InputManager()
        {
            IsAvailable = new(true);
            
            IsAvailable
                .Where(available => !available)
                .Subscribe(_ => Dispose())
                .AddTo(_disposable);
            
            Observable
                .EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ => OnLeftClick?.Invoke())
                .AddTo(_disposable);
            
            Observable
                .EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(1))
                .Subscribe(_ => OnRightClick?.Invoke())
                .AddTo(_disposable);
        }

        public float GetAxisVertical()
        {
            return Input.GetAxisRaw("Vertical");
        }

        public float GetAxisHorizontal()
        {
            return Input.GetAxisRaw("Horizontal");
        }

        private void Dispose()
        {
            _disposable.Dispose();
        }
    }
}