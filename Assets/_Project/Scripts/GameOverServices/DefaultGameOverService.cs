using System;
using R3;

namespace _Project.Scripts.GameOverServices
{
    public class DefaultGameOverService : IDisposable
    {
        public readonly ReactiveProperty<bool> IsGameOver = new(false);
        public readonly Subject<Unit> OnGameOver = new();

        private readonly CompositeDisposable _disposable = new();
        
        public DefaultGameOverService()
        {
            OnGameOver
                .Select(_ => true)
                .Subscribe(isOver => IsGameOver.Value = isOver)
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}