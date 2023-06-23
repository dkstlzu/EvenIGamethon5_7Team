using System;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Leaderboards;

namespace EvenI7
{
    public class LeaderboardSample : MonoBehaviour
    {
        public const string LeaderboardId = "Rankers";
        public string VersionId;
        
        private async void Awake()
        {
            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += () =>
            {
                print($"Signed in Unity Service as {AuthenticationService.Instance.PlayerId}");
            };
            AuthenticationService.Instance.SignInFailed += (s) =>
            {
                print(s);
            };
            
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        
        [ContextMenu("AddScores")]
        public async void AddScore()
        {
            var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, 102);
            Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        }

        [ContextMenu("GetScores")]
        public async void GetScores()
        {
            var scoresResponse =
                await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
            Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        }

        [ContextMenu("GetPaginatedScore")]
        public async void GetPaginatedScores()
        {
            var scoresResponse =
                await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions{Offset = 10, Limit = 10});
            Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        }

        [ContextMenu("GetPlayerScore")]
        public async void GetPlayerScore()
        {
            var scoreResponse = 
                await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
            Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        }

        [ContextMenu("GetVersionScores")]
        public async void GetVersionScores()
        {
            var versionScoresResponse =
                await LeaderboardsService.Instance.GetVersionScoresAsync(LeaderboardId, VersionId);
            Debug.Log(JsonConvert.SerializeObject(versionScoresResponse));
        }
    }
}