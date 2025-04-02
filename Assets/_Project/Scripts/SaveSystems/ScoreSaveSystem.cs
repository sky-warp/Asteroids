using System;
using UnityEngine;

namespace _Project.Scripts.SaveSystems
{
    public class ScoreSaveSystem : Zenject.IInitializable, IDisposable
    {
        public SaveData SaveData { get; }

        public ScoreSaveSystem(SaveData saveData)
        {
            SaveData = saveData;
        }

        public void Initialize()
        {
            string tempJson = PlayerPrefs.GetString("key", "");

            if (!string.IsNullOrEmpty(tempJson))
            {
                var data = JsonUtility.FromJson<SaveData>(tempJson);
                SaveData.SaveHighScoreData(data.HighScore);
            }

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
            string json = JsonUtility.ToJson(SaveData);

            PlayerPrefs.SetString("key", json);

            Debug.Log($"High Score: {PlayerPrefs.GetString("key")}");
        }
    }
}