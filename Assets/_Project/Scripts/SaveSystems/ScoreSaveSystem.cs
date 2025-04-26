using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using R3;
using UnityEngine;

namespace _Project.Scripts.SaveSystems
{
    public class ScoreSaveSystem : IDisposable, ISaveable
    {
        public ScoreSaveData ScoreSaveData { get; }
        private CloudSaveSystem _cloudSaveSystem;
        private CompositeDisposable _disposable = new();

        private string _tempJSON;
        private ScoreSaveData _localData = new();

        public ScoreSaveSystem(ScoreSaveData scoreSaveData, CloudSaveSystem cloudSaveSystem)
        {
            ScoreSaveData = scoreSaveData;
            _cloudSaveSystem = cloudSaveSystem;
        }

        public void Init()
        {
            _tempJSON = PlayerPrefs.GetString("key", "");

            if (_tempJSON == "")
            {
                _localData.SetDefaultValues();
            }
            else
            {
                _localData = JsonUtility.FromJson<ScoreSaveData>(_tempJSON);
            }

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                InitializeLocalSave();
            }
            else
            {
                InitializeRemoteSave();
            }
        }

        public void InitializeLocalSave()
        {
            ScoreSaveData.SetHighScoreData(_localData.HighScore);
            ScoreSaveData.SaveDate(_localData.LastSaveDate);
            ScoreSaveData.SaveTime(_localData.LastSaveTime);

            Observable
                .FromEvent(scoreChanged => ScoreSaveData.OnHighScoreChanged += scoreChanged,
                    scoreChanged => ScoreSaveData.OnHighScoreChanged -= scoreChanged)
                .Subscribe(_ => UpdateHighScoreLocally())
                .AddTo(_disposable);
            Observable
                .FromEvent(scoreChanged => ScoreSaveData.OnHighScoreReset += scoreChanged,
                    scoreChanged => ScoreSaveData.OnHighScoreReset -= scoreChanged)
                .Subscribe(_ => UpdateHighScoreLocally())
                .AddTo(_disposable);
        }

        public async void InitializeRemoteSave()
        {
            await _cloudSaveSystem.Authenticate();

            var (cloudHighScore, cloudDate, cloudTime) = await UniTask.WhenAll(
                _cloudSaveSystem.LoadHighScore(),
                _cloudSaveSystem.LoadLastDate(),
                _cloudSaveSystem.LoadLastTime()
            );

            DateTime localDateTime;
            DateTime cloudDateTime;

            localDateTime = DateTime.Parse(_localData.LastSaveDate + " " + _localData.LastSaveTime);
            cloudDateTime = DateTime.Parse(cloudDate + " " + cloudTime);

            if (localDateTime > cloudDateTime)
            {
                InitializeLocalSave();
                return;
            }

            ScoreSaveData.SetHighScoreData(cloudHighScore);
            ScoreSaveData.SaveDate(cloudDate);
            ScoreSaveData.SaveTime(cloudTime);

            Observable
                .FromEvent(scoreChanged => ScoreSaveData.OnHighScoreChanged += scoreChanged,
                    scoreChanged => ScoreSaveData.OnHighScoreChanged -= scoreChanged)
                .Subscribe(_ => UpdateHighScoreCloud())
                .AddTo(_disposable);
            Observable
                .FromEvent(scoreChanged => ScoreSaveData.OnHighScoreReset += scoreChanged,
                    scoreChanged => ScoreSaveData.OnHighScoreReset -= scoreChanged)
                .Subscribe(_ => UpdateHighScoreCloud())
                .AddTo(_disposable);
        }

        public void ResetParticularSaveData()
        {
            ScoreSaveData.ClearHighScoreData();
        }

        private void UpdateHighScoreLocally()
        {
            ScoreSaveData.SaveDate(DateTime.Now.ToShortDateString());
            ScoreSaveData.SaveTime(DateTime.Now.ToLongTimeString());

            string json = JsonUtility.ToJson(ScoreSaveData);

            PlayerPrefs.SetString("key", json);

            Debug.Log($"High Score: {PlayerPrefs.GetString("key")}");
        }

        private async void UpdateHighScoreCloud()
        {
            ScoreSaveData.SaveDate(DateTime.Now.ToShortDateString());
            ScoreSaveData.SaveTime(DateTime.Now.ToLongTimeString());

            string json = JsonUtility.ToJson(ScoreSaveData);

            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            await _cloudSaveSystem.SaveData(values);

            Debug.Log($"High Score: {ScoreSaveData.HighScore}");
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}