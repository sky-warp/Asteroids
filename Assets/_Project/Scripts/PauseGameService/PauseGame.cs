using R3;

namespace _Project.Scripts.PauseGameService
{
    public class PauseGame
    {
        public readonly ReactiveProperty<bool> IsPaused = new(false);
        public readonly Subject<Unit> OnPause = new();

        private CompositeDisposable _disposable = new();
        
        public PauseGame()
        {
            OnPause
                .Select(_ => true)
                .Subscribe(isPaused => IsPaused.Value = isPaused)
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}