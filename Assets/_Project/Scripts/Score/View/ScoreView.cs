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
        [Header("Pause service")] [SerializeField]
        private PauseGameService.PauseGameService _pauseGameService;
        
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _finalScoreText;
        
        [SerializeField] private EnvironmentUnitSpawnService _environmentUnitSpawnService;
        
        [SerializeField] private GameObject _gameOverWindow;
        [SerializeField] private Button _restartButton;
        
        [SerializeField] private SceneManager.SceneManager _sceneManager;

        private ScoreViewModel _scoreViewModel;

        public void Init(ScoreViewModel scoreViewModel)
        {
            _scoreViewModel = scoreViewModel;
            
            _environmentUnitSpawnService.OnScoreChanged
                .Subscribe(_scoreViewModel.IncreaseScore)
                .AddTo(this);
            _pauseGameService.OnPause
                .Subscribe(_ => ShowGameOverWindow())
                .AddTo(this);
            
            _scoreViewModel.CurrentScoreView
                .Subscribe(UpdateScoreText)
                .AddTo(this);
            
            _restartButton.onClick.AddListener(OnRestartGame);
        }

        private void UpdateScoreText(int score)
        {
            _scoreText.text = $"SCORE: {score.ToString()}";
        }

        private void ShowGameOverWindow()
        {
            _gameOverWindow.SetActive(true);
            _finalScoreText.text = $"FINAL SCORE: {_scoreViewModel.CurrentScoreView.Value.ToString()}";
            
            _pauseGameService.PauseService(_environmentUnitSpawnService);
        }

        private void OnRestartGame()
        {
            _sceneManager.RestartGame();
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(OnRestartGame);
        }
    }
}