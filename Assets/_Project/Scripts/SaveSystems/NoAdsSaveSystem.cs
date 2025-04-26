using System;
using R3;
using UnityEngine;

namespace _Project.Scripts.SaveSystems
{
    public class NoAdsSaveSystem : IDisposable
    {
        public NoAdsSaveData NoAdsSaveData { get; }

        private NoAdsSaveData _localNoAdsSaveData = new();
        private string _tempJSON;
        private CompositeDisposable _disposable = new();

        public NoAdsSaveSystem(NoAdsSaveData noAdsSaveData)
        {
            NoAdsSaveData = noAdsSaveData;
        }

        public void Init()
        {
            _tempJSON = PlayerPrefs.GetString("NoAdsWasPurchased", "");

            if (_tempJSON == "")
            {
                _localNoAdsSaveData.SetDefaultStatus();
            }

            _localNoAdsSaveData = JsonUtility.FromJson<NoAdsSaveData>(_tempJSON);

            InitializeLocalSave();
        }

        private void InitializeLocalSave()
        {
            NoAdsSaveData.SetPurchaseStatus(_localNoAdsSaveData.PurchaseStatus);

            Observable
                .FromEvent(wasBuyed => NoAdsSaveData.WasPurchased += wasBuyed,
                    wasBuyed => NoAdsSaveData.WasPurchased -= wasBuyed)
                .Subscribe(_ => UpdateNoAdsProductStatusLocally())
                .AddTo(_disposable);
        }

        public void UpdateNoAdsProductStatusLocally()
        {
            string json = JsonUtility.ToJson(NoAdsSaveData);

            PlayerPrefs.SetString("NoAdsWasPurchased", json);
        }

        public bool CheckBuyStatus()
        {
            return NoAdsSaveData.CheckStatus();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}