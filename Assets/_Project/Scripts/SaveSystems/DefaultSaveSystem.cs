using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using R3;
using UnityEngine;

namespace _Project.Scripts.SaveSystems
{
    public class DefaultSaveSystem : IDisposable, ISaveable
    {
        public SaveData SavedData { get; }
        private CloudSaveSystem _cloudSaveSystem;
        private CompositeDisposable _disposable = new();

        private string _tempJSON;
        private SaveData _localData = new();

        public DefaultSaveSystem(SaveData savedData, CloudSaveSystem cloudSaveSystem)
        {
            SavedData = savedData;
            _cloudSaveSystem = cloudSaveSystem;
        }

        public async UniTask Init()
        {
            _tempJSON = PlayerPrefs.GetString("key", "");

            if (_tempJSON == "")
            {
                _localData.SetDefaultValues();
            }
            else
            {
                _localData = JsonUtility.FromJson<SaveData>(_tempJSON);
            }

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                InitializeLocalSave();
            }
            else
            {
                await InitializeRemoteSave();
            }
        }

        public void InitializeLocalSave()
        {
            SavedData.SetHighScoreData(_localData.HighScore);
            SavedData.SaveDate(_localData.LastSaveDate);
            SavedData.SaveTime(_localData.LastSaveTime);
            SavedData.SetPurchaseStatus(_localData.NoAdsPurchaseStatus);

            Observable
                .FromEvent(scoreChanged => SavedData.OnHighScoreChanged += scoreChanged,
                    scoreChanged => SavedData.OnHighScoreChanged -= scoreChanged)
                .Subscribe(_ => UpdateDataLocally())
                .AddTo(_disposable);
            Observable
                .FromEvent(scoreChanged => SavedData.OnHighScoreReset += scoreChanged,
                    scoreChanged => SavedData.OnHighScoreReset -= scoreChanged)
                .Subscribe(_ => UpdateDataLocally())
                .AddTo(_disposable);

            Observable
                .FromEvent(noAdsPurchased => SavedData.OnNoAdsPurchased += noAdsPurchased,
                    noAdsPurchased => SavedData.OnNoAdsPurchased -= noAdsPurchased)
                .Subscribe(_ => UpdateDataLocally())
                .AddTo(_disposable);
            Observable
                .FromEvent(noAdsPurchased => SavedData.OnNoAdsReseted += noAdsPurchased,
                    noAdsPurchased => SavedData.OnNoAdsReseted -= noAdsPurchased)
                .Subscribe(_ => UpdateDataLocally())
                .AddTo(_disposable);
        }

        public async UniTask InitializeRemoteSave()
        {
            await _cloudSaveSystem.Authenticate();

            var (cloudHighScore, cloudDate, cloudTime, noAdsStatus) = await UniTask.WhenAll(
                _cloudSaveSystem.LoadHighScore(),
                _cloudSaveSystem.LoadLastDate(),
                _cloudSaveSystem.LoadLastTime(),
                _cloudSaveSystem.LoadNoAdsPurchaseStatus()
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

            SavedData.SetHighScoreData(cloudHighScore);
            SavedData.SaveDate(cloudDate);
            SavedData.SaveTime(cloudTime);
            SavedData.SetPurchaseStatus(noAdsStatus);

            Observable
                .FromEvent(scoreChanged => SavedData.OnHighScoreChanged += scoreChanged,
                    scoreChanged => SavedData.OnHighScoreChanged -= scoreChanged)
                .Subscribe(_ => UpdateDataRemotely())
                .AddTo(_disposable);
            Observable
                .FromEvent(scoreChanged => SavedData.OnHighScoreReset += scoreChanged,
                    scoreChanged => SavedData.OnHighScoreReset -= scoreChanged)
                .Subscribe(_ => UpdateDataRemotely())
                .AddTo(_disposable);

            Observable
                .FromEvent(noAdsPurchased => SavedData.OnNoAdsPurchased += noAdsPurchased,
                    noAdsPurchased => SavedData.OnNoAdsPurchased -= noAdsPurchased)
                .Subscribe(_ => UpdateDataRemotely())
                .AddTo(_disposable);
            Observable
                .FromEvent(noAdsPurchased => SavedData.OnNoAdsReseted += noAdsPurchased,
                    noAdsPurchased => SavedData.OnNoAdsReseted -= noAdsPurchased)
                .Subscribe(_ => UpdateDataRemotely())
                .AddTo(_disposable);
        }

        public void ResetHighScore()
        {
            SavedData.ClearHighScoreData();
        }

        public void ResetNoAdsPurchased()
        {
            SavedData.ResetPurchaseStatus();
        }

        private void UpdateDataLocally()
        {
            SavedData.SaveDate(DateTime.Now.ToShortDateString());
            SavedData.SaveTime(DateTime.Now.ToLongTimeString());

            string json = JsonUtility.ToJson(SavedData);

            PlayerPrefs.SetString("key", json);

            Debug.Log($"High Score: {PlayerPrefs.GetString("key")}");
        }

        private async void UpdateDataRemotely()
        {
            SavedData.SaveDate(DateTime.Now.ToShortDateString());
            SavedData.SaveTime(DateTime.Now.ToLongTimeString());

            string json = JsonUtility.ToJson(SavedData);

            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            await _cloudSaveSystem.SaveData(values);

            Debug.Log($"High Score: {SavedData.HighScore}");
        }

        public bool CheckBuyStatus()
        {
            return SavedData.CheckStatus();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}