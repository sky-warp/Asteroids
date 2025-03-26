using System;
using _Project.Scripts.AudioSystems;
using R3;
using UnityEngine;

namespace _Project.Scripts.InputService
{
    public class InputManager : IInputable
    {
        public ReactiveProperty<bool> IsAvailable { get; }
        public event Action OnBulletRelease;
        public event Action OnLaserRelease;
        
        private readonly CompositeDisposable _disposable = new();

        private DefaultAudioManager _audioManager;
        
        public InputManager(DefaultAudioManager audioManager)
        {
            _audioManager = audioManager;
            
            IsAvailable = new(true);
            
            IsAvailable
                .Where(available => !available)
                .Subscribe(_ => Dispose())
                .AddTo(_disposable);
            
            Observable
                .EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ => OnBulletRelease?.Invoke())
                .AddTo(_disposable);
            
            Observable
                .EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(1))
                .Subscribe(_ => OnLaserRelease?.Invoke())
                .AddTo(_disposable);

            OnBulletRelease += _audioManager.PlayBulletSound;
            OnLaserRelease += _audioManager.PlayLaserSound;
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
            
            OnBulletRelease -= _audioManager.PlayBulletSound;
            OnLaserRelease -= _audioManager.PlayLaserSound;
        }
    }
}