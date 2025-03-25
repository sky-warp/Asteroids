using System;

namespace _Project.Scripts.Infrastructure
{
    public class GameEventManager
    {
        public event Action OnGameStart;

        public void OnStartGame()
        {
            OnGameStart?.Invoke();
        }
    }
}