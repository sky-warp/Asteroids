using _Project.Scripts.Score.ViewModel;
using _Project.Scripts.SpawnService;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Score.View
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _finalScoreText;

        [SerializeField] private GameObject _gameOverWindow;
        [SerializeField] private Button _restartButton;

        private SceneManager.SceneManager _sceneManager;

        private ScoreViewModel _scoreViewModel;

        public void Init(ScoreViewModel scoreViewModel, EnvironmentUnitSpawnService environmentUnitSpawnService,
            PauseGameService.PauseGameService pauseGameService)
        {
            _sceneManager = new();
            _scoreViewModel = scoreViewModel;

            environmentUnitSpawnService.BigAsteroidScore
                .Subscribe(_scoreViewModel.IncreaseScore)
                .AddTo(this);
            environmentUnitSpawnService.SmallAsteroidScore
                .Subscribe(_scoreViewModel.IncreaseScore)
                .AddTo(this);
            environmentUnitSpawnService.UfoScore
                .Subscribe(_scoreViewModel.IncreaseScore)
                .AddTo(this);

            _scoreViewModel.CurrentScoreView
                .Subscribe(UpdateScoreText)
                .AddTo(this);

            pauseGameService.OnPause
                .Subscribe(_ => ShowGameOverWindow())
                .AddTo(this);
            
            _restartButton.
                OnClickAsObservable()
                .Subscribe(_ => OnRestartGame())
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
            _scoreText.gameObject.SetActive(false);
            _gameOverWindow.SetActive(true);
            _finalScoreText.text = $"FINAL SCORE: {_scoreViewModel.CurrentScoreView.Value.ToString()}";
        }

        private void OnRestartGame()
        {
            _sceneManager.RestartGame();
        }
    }
}