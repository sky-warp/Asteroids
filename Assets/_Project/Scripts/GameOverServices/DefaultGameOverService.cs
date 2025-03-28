using R3;

namespace _Project.Scripts.GameOverServices
{
    public class DefaultGameOverService
    {
        public readonly Subject<Unit> OnGameOver = new();
    }
}