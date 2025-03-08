using R3;

namespace _Project.Scripts.InputService
{
    public class InputManager
    {
        public readonly Subject<Unit> OnLeftClick = new Subject<Unit>();
        public readonly Subject<Unit> OnRightClick = new Subject<Unit>();
    }
}