using System;
using R3;
using UnityEngine.Device;
using Zenject;

namespace _Project.Scripts.GameStateServices.PauseMenu
{
    public class PauseMenuController : IInitializable, IDisposable
    {
        private DefaultGameStateService _defaultGameStateService;
        private PauseMenuView _pauseMenuView;
        private CompositeDisposable _disposable = new();
        private bool _isPaused;

        public PauseMenuController(DefaultGameStateService defaultGameStateService, PauseMenuView pauseMenuView)
        {
            _defaultGameStateService = defaultGameStateService;
            _pauseMenuView = pauseMenuView;
        }

        public void Initialize()
        {
            _defaultGameStateService.OnGamePaused
                .Subscribe(_ => PauseOrResume())
                .AddTo(_disposable);
            
            _pauseMenuView.ResumeButton.OnClickAsObservable()
                .Subscribe(_ => PauseOrResume())
                .AddTo(_disposable);
            
            _pauseMenuView.ExitButton.OnClickAsObservable()
                .Subscribe(_ => ExitApplication())
                .AddTo(_disposable);
        }

        private void PauseOrResume()
        {
            if (!_isPaused)
            {
                _pauseMenuView.ShowPauseMenu();
                _isPaused = true;
            }
            else
            {
                _pauseMenuView.HidePauseMenu();
                _isPaused = false;
                _defaultGameStateService.OnGameResume.OnNext(Unit.Default);
            }
        }

        private void ExitApplication()
        {
            Application.Quit();
        }
        
        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}