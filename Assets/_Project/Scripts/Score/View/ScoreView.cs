using _Project.Scripts.DOTweenAnimations;
using _Project.Scripts.Score.ViewModel;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.Score.View
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _finalScoreText;

        [SerializeField] private GameObject _gameOverWindow;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _resetHighScoreButton;

        private ScoreViewModel _scoreViewModel;

        private EndGameWindowAppearAnimation _endGameWindowAppearAnimation;
        
        private CompositeDisposable _shortAdDisposable = new();
        
        [Inject]
        public void Init(ScoreViewModel scoreViewModel, EndGameWindowAppearAnimation endGameWindowAppearAnimation)
        {
            _scoreViewModel = scoreViewModel;
            _endGameWindowAppearAnimation = endGameWindowAppearAnimation;
            
            _scoreViewModel.CurrentScoreView
                .Subscribe(UpdateScoreText)
                .AddTo(this);
            _scoreViewModel.IsGameOver
                .Where(isGameOver => isGameOver)
                .Subscribe(_ => ShowGameOverWindow())
                .AddTo(this);
            _scoreViewModel.IsGameResume
                .Where(isGameResume => isGameResume)
                .Subscribe(_ => HideGameOverWindow())
                .AddTo(this);
            _scoreViewModel.IAPController.IsNoAds
                .Where(value => value)
                .Subscribe(_ => _shortAdDisposable?.Dispose())
                .AddTo(this);

            _resetHighScoreButton.OnClickAsObservable()
                .Subscribe(_ => _scoreViewModel.ResetHighScoreView())
                .AddTo(this);
            
            _restartButton.OnClickAsObservable()
                .Subscribe(_ => _scoreViewModel.AdManager.ShowShortAd())
                .AddTo(_shortAdDisposable); 
            _restartButton.OnClickAsObservable()
                .Where(_ => scoreViewModel.IAPController.IsNoAds.Value)
                .Subscribe(_ => _scoreViewModel.OnRestartGame())
                .AddTo(this);

            _scoreText.gameObject.SetActive(true);
        }
        
        private void UpdateScoreText(int score)
        {
            _scoreText.text = $"SCORE: {score.ToString()}";
        }

        private void ShowGameOverWindow()
        {
            gameObject.SetActive(false);
            
            _gameOverWindow.SetActive(true);
            _endGameWindowAppearAnimation.ShowGameOverWindow();
            
            _scoreText.gameObject.SetActive(false);
            _finalScoreText.text = $"FINAL SCORE: {_scoreViewModel.CurrentScoreView.Value.ToString()}";
        }

        private void HideGameOverWindow()
        {
            gameObject.SetActive(true);
            
            _gameOverWindow.SetActive(false);
            
            _scoreText.gameObject.SetActive(true);
        }
        
        private void OnDestroy()
        {
            _resetHighScoreButton.onClick.RemoveAllListeners();
            _shortAdDisposable?.Dispose();
        }
    }
}