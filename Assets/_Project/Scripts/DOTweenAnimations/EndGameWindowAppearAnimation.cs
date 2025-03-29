using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.DOTweenAnimations
{
    public class EndGameWindowAppearAnimation : MonoBehaviour
    {
        private Tween _tween;
        
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        public void ShowGameOverWindow()
        {
            _tween = _canvasGroup.DOFade(1, 0.5f);
        }

        private void OnDestroy()
        {
            _tween.Kill();
        }
    }
}