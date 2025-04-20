using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using R3;
using Unity.Services.Core;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.SaveSystems
{
    public class ScoreSaveSystem : IInitializable, IDisposable
    {
        public SaveData SaveData { get; }
        private CloudSave _cloudSave;
        private CompositeDisposable _disposable = new();

        public ScoreSaveSystem(SaveData saveData, CloudSave cloudSave)
        {
            SaveData = saveData;
            _cloudSave = cloudSave;
        }

        public void Initialize()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                InitializeLocalSave();
            }
            else
            {
                InitializeRemoteSave();
            }
        }

        private void InitializeLocalSave()
        {
            string tempJson = PlayerPrefs.GetString("key", "");

            if (!string.IsNullOrEmpty(tempJson))
            {
                var data = JsonUtility.FromJson<SaveData>(tempJson);
                SaveData.SetHighScoreData(data.HighScore);
                SaveData.SaveDate(data.LastSaveDate);
                SaveData.SaveTime(data.LastSaveTime);
            }

            Observable
                .FromEvent(scoreChanged => SaveData.OnHighScoreChanged += scoreChanged,
                    scoreChanged => SaveData.OnHighScoreChanged -= scoreChanged)
                .Subscribe(_ => UpdateHighScoreLocally())
                .AddTo(_disposable);
            Observable
                .FromEvent(scoreChanged => SaveData.OnHighScoreReset += scoreChanged,
                    scoreChanged => SaveData.OnHighScoreReset -= scoreChanged)
                .Subscribe(_ => UpdateHighScoreLocally())
                .AddTo(_disposable);
        }

        private async void InitializeRemoteSave()
        {
            await UnityServices.InitializeAsync();
            await _cloudSave.Authenticate();

            var (cloudHighScore, cloudDate, cloudTime) = await UniTask.WhenAll(
                _cloudSave.LoadHighScore(),
                _cloudSave.LoadLastDate(),
                _cloudSave.LoadLastTime()
            );

            SaveData.SetHighScoreData(cloudHighScore);
            SaveData.SaveDate(cloudDate);
            SaveData.SaveTime(cloudTime);

            Observable
                .FromEvent(scoreChanged => SaveData.OnHighScoreChanged += scoreChanged,
                    scoreChanged => SaveData.OnHighScoreChanged -= scoreChanged)
                .Subscribe(_ => UpdateHighScoreCloud())
                .AddTo(_disposable);
            Observable
                .FromEvent(scoreChanged => SaveData.OnHighScoreReset += scoreChanged,
                    scoreChanged => SaveData.OnHighScoreReset -= scoreChanged)
                .Subscribe(_ => UpdateHighScoreCloud())
                .AddTo(_disposable);
        }
        
        public void ResetHighScore()
        {
            SaveData.ClearHighScoreData();
        }

        private void UpdateHighScoreLocally()
        {
            SaveData.SaveDate(DateTime.Now.ToShortDateString());
            SaveData.SaveTime(DateTime.Now.ToLongTimeString());

            string json = JsonUtility.ToJson(SaveData);

            PlayerPrefs.SetString("key", json);

            Debug.Log($"High Score: {PlayerPrefs.GetString("key")}");
        }

        private async void UpdateHighScoreCloud()
        {
            SaveData.SaveDate(DateTime.Now.ToShortDateString());
            SaveData.SaveTime(DateTime.Now.ToLongTimeString());

            string json = JsonUtility.ToJson(SaveData);

            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            await _cloudSave.SaveData(values);

            Debug.Log($"High Score: {SaveData.HighScore}");
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}