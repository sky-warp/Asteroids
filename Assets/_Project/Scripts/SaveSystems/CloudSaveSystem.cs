using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine;

namespace _Project.Scripts.SaveSystems
{
    public class CloudSaveSystem
    {
        public const string HIGH_SCORE_KEY = "HighScore";
        public const string LAST_DATE_KEY = "LastSaveDate";
        public const string LAST_TIME_KEY = "LastSaveTime";

        public async UniTask Authenticate()
        {
            await AuthPlayer();
        }

        private async UniTask AuthPlayer()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Player ID: {AuthenticationService.Instance.PlayerId}");
            }
            catch (AuthenticationException e)
            {
                Debug.Log(e);
                throw;
            }
        }

        public async UniTask SaveData(Dictionary<string, object> result)
        {
            await CloudSaveService.Instance.Data.Player.SaveAsync(result);
        }

        public async UniTask<int> LoadHighScore()
        {
            int result = 0;

            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>
            {
                HIGH_SCORE_KEY
            });

            if (playerData.TryGetValue(HIGH_SCORE_KEY, out var score))
            {
                result = score.Value.GetAs<int>();
            }

            return result;
        }

        public async UniTask<string> LoadLastDate()
        {
            string result = "";

            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>
            {
                LAST_DATE_KEY
            });

            if (playerData.TryGetValue(LAST_DATE_KEY, out var score))
            {
                result = score.Value.GetAs<string>();
            }

            return result;
        }

        public async UniTask<string> LoadLastTime()
        {
            string result = "";

            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>
            {
                LAST_TIME_KEY
            });

            if (playerData.TryGetValue(LAST_TIME_KEY, out var score))
            {
                result = score.Value.GetAs<string>();
            }

            return result;
        }
    }
}