using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class LoadingUI : UI
    {
        public Button Button;
        public TextMeshProUGUI GoogleText;
        public TextMeshProUGUI ProgressText;
        public TextMeshProUGUI QuestText;

        private const string PROGRESS = "진행 데이터";
        private const string QUEST = "퀘스트 데이터";
        private const string LOADED = " 불러옴";
        private const string LOAD_FAILED = " 불러오는데 실패함";

        protected override void Awake()
        {
            base.Awake();

#if UNITY_EDITOR
            GameManager.instance.SaveLoadSystem.OnSaveDataLoaded += () =>
            {
                ProgressText.text = PROGRESS + LOADED;
            };
            
            QuestManager.instance.SaveLoadSystem.OnSaveDataLoaded += () =>
            {
                QuestText.text = QUEST + LOADED;
            };
#else
            GoogleManager.instance.OnDataLoadSuccess += () =>
            {
                ProgressText.text = PROGRESS + LOADED;
                QuestText.text = QUEST + LOADED;
            };

            GoogleManager.instance.OnDataLoadFail += () =>
            {
                GoogleText.text = "데이터를 불러오는데 실패했습니다. 버튼을 눌러 다시 시도해 주세요.";
                ProgressText.text = PROGRESS + LOAD_FAILED;
                QuestText.text = QUEST + LOAD_FAILED;

                Button.interactable = true;
                Button.onClick.AddListener(OnLoadSaveDataButtonClicked);
            };

            GoogleManager.instance.OnSelectUIUnselected += (status) =>
            {
                GoogleText.text = "데이터를 불러오는데 실패했습니다. 버튼을 눌러 다시 시도해 주세요.";
                ProgressText.text = PROGRESS + LOAD_FAILED;
                QuestText.text = QUEST + LOAD_FAILED;

                Button.interactable = true;
                Button.onClick.AddListener(OnLoadSaveDataButtonClicked);
            };
#endif
            
            GoogleManager.instance.OnAuthentication += (status) =>
            {
                switch (status)
                {
                    case SignInStatus.Success:
                        GoogleText.text = $"로그인 성공! 게임을 시작합니다. \n {PlayGamesPlatform.Instance.localUser.userName}, {Social.localUser.id}";
                        break;
                    case SignInStatus.UiSignInRequired:
                    case SignInStatus.DeveloperError:
                    case SignInStatus.NetworkError:
                    case SignInStatus.InternalError:
                    case SignInStatus.Canceled:
                    case SignInStatus.AlreadyInProgress:
                    case SignInStatus.Failed:
                    case SignInStatus.NotAuthenticated:
                        GoogleText.text = $"로그인에 실패했습니다. {status} 버튼을 눌러 다시 로그인을 시도해 주세요.";
                        Button.interactable = true;
                        Button.onClick.AddListener(OnLoginButtonClicked);
                        break;
                }
            };


#if UNITY_EDITOR
            GoogleManager.instance.OnAuthentication += (status) => CheckReady();
            GameManager.instance.SaveLoadSystem.OnSaveDataLoaded += CheckReady;
            QuestManager.instance.SaveLoadSystem.OnSaveDataLoaded += CheckReady;
#else
            GoogleManager.instance.OnDataLoadSuccess += CheckReady;
#endif

        }

        private void CheckReady()
        {
#if UNITY_EDITOR
            if (GoogleManager.instance.SignInStatus == SignInStatus.Success && GameManager.instance.SaveLoadSystem.DataIsLoaded &&
                QuestManager.instance.SaveLoadSystem.DataIsLoaded)
            {
                LoadingScene.LoadScene(SceneName.Scenario, minimumDelayTime:3);
            }
#else
            if (GameManager.instance.SaveLoadSystem.DataIsLoaded && QuestManager.instance.SaveLoadSystem.DataIsLoaded)
            {
                LoadingScene.LoadScene(SceneName.Scenario, minimumDelayTime:3);
            }
#endif

        }

        public void OnLoginButtonClicked()
        {
            GoogleManager.instance.Login(SignInInteractivity.CanPromptAlways);
            Button.onClick.RemoveAllListeners();
            Button.interactable = false;
        }
        
        private void OnLoadSaveDataButtonClicked()
        {
            GoogleManager.instance.Load();
            Button.onClick.RemoveAllListeners();
            Button.interactable = false;
        }
    }
}