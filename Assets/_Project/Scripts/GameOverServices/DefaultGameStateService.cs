using R3;

namespace _Project.Scripts.GameOverServices
{
    public class DefaultGameStateService
    {
        public readonly Subject<Unit> OnGameOver = new();
        public readonly Subject<Unit> OnGameStart = new();
        public readonly Subject<Unit> OnGameResume = new();
    }
}