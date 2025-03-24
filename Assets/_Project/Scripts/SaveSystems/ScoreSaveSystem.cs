using System;
using R3;
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

            PlayerPrefs.SetString("HighScore", json);

            Debug.Log($"High Score: {PlayerPrefs.GetString("HighScore")}");
        }
    }
}