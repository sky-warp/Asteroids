using System;

namespace _Project.Scripts.SaveSystems
{
    public class SaveData
    {
        public event Action OnHighScoreChanged;
        public int HighScore { get; private set; }

        public void SaveHighScoreData(int highScore)
        {
            if (highScore > HighScore)
                HighScore = highScore;

            OnHighScoreChanged?.Invoke();
        }

        public void CleaHighScoreData()
        {
            HighScore = 0;
        }
    }
}