using UnityEngine;
using R3;

namespace _Project.Scripts.InputService
{
    public class InputManager : MonoBehaviour
    {
        [Header("Pause service")] [SerializeField]
        private PauseGameService.PauseGameService _pauseGameService;

        public readonly Subject<Unit> OnLeftClick = new Subject<Unit>();
        public readonly Subject<Unit> OnRightClick = new Subject<Unit>();

        private void Awake()
        {
            _pauseGameService.OnPause
                .Subscribe(_ => _pauseGameService.PauseService(this))
                .AddTo(this);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                OnLeftClick.OnNext(Unit.Default);

            if (Input.GetMouseButtonDown(1))
                OnRightClick?.OnNext(Unit.Default);
        }
    }
}