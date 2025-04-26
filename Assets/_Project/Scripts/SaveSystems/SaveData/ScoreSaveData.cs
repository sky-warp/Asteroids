using System;

namespace _Project.Scripts.SaveSystems
{
    [Serializable]
    public class ScoreSaveData
    {
        public int HighScore;
        public string LastSaveDate;
        public string LastSaveTime;
        public event Action OnHighScoreChanged;
        public event Action OnHighScoreReset;

        public int SetHighScoreData(int highScore)
        {
            if (highScore > HighScore)
            {
                HighScore = highScore;

                OnHighScoreChanged?.Invoke();
            }
            
            return HighScore;
        }

        public void ClearHighScoreData()
        {
            HighScore = 0;
            OnHighScoreReset?.Invoke();
        }

        public void SaveDate(string date)
        {
            LastSaveDate = date;
        }

        public void SaveTime(string time)
        {
            LastSaveTime = time;
        }

        public void SetDefaultValues()
        {
            HighScore = 0;
            LastSaveDate = DateTime.Now.ToString();
            LastSaveTime = DateTime.Now.ToString();
        }
    }
}