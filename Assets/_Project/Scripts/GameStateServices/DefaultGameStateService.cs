using R3;

namespace _Project.Scripts.GameStateServices
{
    public class DefaultGameStateService
    {
        public readonly Subject<Unit> OnGameOver = new();
        public readonly Subject<Unit> OnGameStart = new();
        public readonly Subject<Unit> OnGamePaused = new();
        public readonly Subject<Unit> OnGameResume = new();
    }
}