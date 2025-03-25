using System;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts.SaveSystems
{
    public class ScoreSaveSystem : IDisposable
    {
        public SaveData SaveData { get; }

        public ScoreSaveSystem(SaveData saveData)
        {
            SaveData = saveData;
            SaveData.SaveHighScoreData(PlayerPrefs.GetInt("highScore", 0));
                
            SaveData.OnHighScoreChanged += UpdateHighScore;
        }

        public void ResetHighScore()
        {
            SaveData.CleaHighScoreData();
        }

        public void Dispose()
        {
            SaveData.OnHighScoreChanged -= UpdateHighScore;
        }

        private void UpdateHighScore()
        {
            var json = JsonUtility.ToJson(SaveData.Serialize());

            PlayerPrefs.SetInt("highScore", SaveData.HighScore);
            PlayerPrefs.SetString("key", json);

            Debug.Log($"High Score: {PlayerPrefs.GetString("key")}");
        }
    }
}