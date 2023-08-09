// using System;
// using dkstlzu.Utility;
// using GooglePlayGames;
// using GooglePlayGames.BasicApi;
// using GooglePlayGames.BasicApi.Nearby;
// using MoonBunny.Dev;
// using MoonBunny.UIs;
// using UnityEngine;
// using UnityEngine.SocialPlatforms;
//
// namespace MoonBunny
// {
//     public class GoogleManager : Singleton<GoogleManager>
//     {
//         public LoadingUI LoadingUI;
//         public SignInStatus SignInStatus;
//
//         private PlayGamesClientConfiguration _clientConfiguration;
//
//         public string Token;
//         private IPlayGamesClient _client;
//
//         public event Action OnLoginPlayStoreSuccess;
//         
//         private void Start()
//         {
// #if UNITY_EDITOR
// #else
//             _clientConfiguration = new PlayGamesClientConfiguration.Builder().Build();
//             PlayGamesPlatform.InitializeInstance(_clientConfiguration);
//             PlayGamesPlatform.DebugLogEnabled = true;
// #endif
//             Login();
//         }
//
//         private void AfterInitializeNearby(INearbyConnectionClient nearby)
//         {
//         }
//
//         public void Login(SignInInteractivity interactivity = SignInInteractivity.CanPromptOnce)
//         {
// #if UNITY_EDITOR
//             Social.localUser.Authenticate(ProcessAuthentication);
// #else
//             PlayGamesPlatform.Instance.Authenticate(interactivity, PlayGamesAuthentication);
// #endif
//         }
//
//         private void PlayGamesAuthentication(SignInStatus status)
//         {
//             MoonBunnyLog.print($"PlayGamesPlatform Authentication {status}");
//
//             LoadingUI.Text.text = "로그인중";
//
//             switch (status)
//             {
//                 case SignInStatus.Success:
//                     LoadingUI.Text.text = $"로그인 성공! 게임을 시작합니다. \n {PlayGamesPlatform.Instance.localUser.userName}, {Social.localUser.id}";
//                     
//                     // PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
//                     // {
//                     //     MoonBunnyLog.print($"Authentication code : {code}");
//                     //     Token = code;
//                     // });
//                     OnLoginPlayStoreSuccess?.Invoke();
//                     break;
//                 case SignInStatus.UiSignInRequired:
//                 case SignInStatus.DeveloperError:
//                 case SignInStatus.NetworkError:
//                 case SignInStatus.InternalError:
//                 case SignInStatus.Canceled:
//                 case SignInStatus.AlreadyInProgress:
//                 case SignInStatus.Failed:
//                 case SignInStatus.NotAuthenticated:
//                     LoadingUI.Button.interactable = true;
//                     LoadingUI.Text.text = $"로그인에 실패했습니다. {status} 버튼을 눌러 다시 로그인을 시도해 주세요.";
//                     break;
//             }
//
//             SignInStatus = status;
//         }
//
//         private void ProcessAuthentication(bool success, string result)
//         {
//             MoonBunnyLog.print($"Success {success}, Result {result}");
//
//             if (success)
//             {
//                 LoadingUI.Text.text = $"로그인 성공! 게임을 시작합니다.\n{Social.localUser.userName} {Social.localUser.id}";
//                 SignInStatus = SignInStatus.Success;
//                 OnLoginPlayStoreSuccess?.Invoke();
//             }
//             else
//             {
//                 LoadingUI.Button.interactable = true;
//                 LoadingUI.Text.text = "로그인에 실패했습니다. 버튼을 눌러 다시 로그인을 시도해 주세요.";
//                 SignInStatus = SignInStatus.Canceled;
//             }
//         }
//     }
// }