using System;
using _Project.Scripts.GameStateServices;
using R3;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.InputService
{
    public class InputManager : IInitializable, IDisposable , IInputable
    {
        public ReactiveProperty<bool> IsAvailable { get; } = new(true);
        public event Action OnBulletRelease;
        public event Action OnLaserRelease;
        public event Action OnEscPressed;

        private DefaultGameStateService _defaultGameStateService;
        private readonly CompositeDisposable _disposable = new();

        public InputManager(DefaultGameStateService defaultGameStateService)
        {
            _defaultGameStateService = defaultGameStateService;
        }

        public void Initialize()
        {
            Observable
                .EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0) && IsAvailable.Value)
                .Subscribe(_ => OnBulletRelease?.Invoke())
                .AddTo(_disposable);

            Observable
                .EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(1) && IsAvailable.Value)
                .Subscribe(_ => OnLaserRelease?.Invoke())
                .AddTo(_disposable);

            Observable
                .EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Escape) && IsAvailable.Value)
                .Subscribe(_ => OnEscPressed?.Invoke())
                .AddTo(_disposable);

            Observable
                .FromEvent(onGamePaused => OnEscPressed += onGamePaused,
                    onGamePaused => OnEscPressed -= onGamePaused)
                .Subscribe(_ => _defaultGameStateService.OnGamePaused.OnNext(Unit.Default))
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

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}