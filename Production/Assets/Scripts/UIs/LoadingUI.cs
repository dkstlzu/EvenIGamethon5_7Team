using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GooglePlayGames.BasicApi;
using MoonBunny.Dev;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class LoadingUI : UI
    {
        public TextMeshProUGUI ProgressText;
        public TextMeshProUGUI ResultText;

        public ConfirmUI ConfirmUI;

        public Image HopingBunnyImage;
        public List<Sprite> HopingBunnySpriteList;
        public float HopingBunnyTweenInterval;

        private const string _CANNOT_FIND_SAVE_DATA = "세이브 데이터를 찾을 수 없습니다. 이대로 진행하면 진행상황이 초기화 됩니다. 진행하시겠습니까?";

        private const string _PLAYSTORE_LOGIN = "플레이스토어 로그인중";
        private const string _RETRY_LOGIN = "화면을 탭하여 로그인 재시도";
        private const string _SAVEDATA = "세이브 데이터 불러오는중";
        private const string _SUCCESS = "성공";
        private const string _FAILED = "실패";
        private const string _DOTS = "...";

        private const float TWEEN_DURATION = 0.3f;

        private bool _loginSucceed = false;
        private bool _loadDataSucceed = false;

        protected override void Awake()
        {
            base.Awake();

            GoogleManager.instance.OnSelectUIUnselected += status =>
            {
                ProgressText.DOText(_SAVEDATA + _DOTS + _FAILED, TWEEN_DURATION);
            };

            GoogleManager.instance.OnAuthentication += status =>
            {
                switch (status)
                {
                    case SignInStatus.Success: break;
                    case SignInStatus.UiSignInRequired:
                    case SignInStatus.DeveloperError:
                    case SignInStatus.NetworkError:
                    case SignInStatus.InternalError:
                    case SignInStatus.Canceled:
                    case SignInStatus.AlreadyInProgress:
                    case SignInStatus.Failed:
                    case SignInStatus.NotAuthenticated:
                        break;
                }
            };

            GoogleManager.instance.OnAuthentication += CheckGPGS;
        }

        private void Start()
        {
            ProgressText.text = _PLAYSTORE_LOGIN;
            GoogleManager.instance.Login();
            StartCoroutine(BunnyTween());
        }

        IEnumerator BunnyTween()
        {
            var wait = new WaitForSeconds(HopingBunnyTweenInterval);

            int index = 0;
            
            while (true)
            {
                yield return wait;
                HopingBunnyImage.sprite = HopingBunnySpriteList[index];

                index = (index + 1) % HopingBunnySpriteList.Count;
            }
        }

        private void CheckGPGS(SignInStatus status)
        {
            if (status == SignInStatus.Success)
            {
                MoonBunnyLog.print("PlayStore Login Success.", tag:"LoadingUI");
                _loginSucceed = true;
                ProgressText.DOText(_PLAYSTORE_LOGIN + _DOTS + _SUCCESS, TWEEN_DURATION).OnComplete(() => ProgressText.text = _SAVEDATA);
                GameManager.instance.SaveLoadSystem.OnSaveDataLoaded += CheckSaveDataLoaded;
            } else
            {
                MoonBunnyLog.print("PlayStore Login Fail.", tag:"LoadingUI");
                _loginSucceed = false;
                ProgressText.DOText(_PLAYSTORE_LOGIN + _DOTS + _FAILED, TWEEN_DURATION)
                    .OnComplete(() => ProgressText.DOText(_RETRY_LOGIN, TWEEN_DURATION));
                InputManager.instance.InputAsset.Ingame.TouchPressed.performed += TapAnywhere;
            }
        }

        private void TapAnywhere(InputAction.CallbackContext context)
        {
            if (!_loginSucceed)
            {
                GoogleManager.instance.Login();
            } else
            {
                GoogleManager.instance.Load();
            }
        }

        private void CheckSaveDataLoaded()
        {
            if (GameManager.instance.SaveLoadSystem.DataIsLoaded)
            {
                MoonBunnyLog.print("ProgressSaveData load Success.", tag:"LoadingUI");
                ProgressText.DOText(_SAVEDATA + _DOTS, TWEEN_DURATION);
                QuestManager.instance.SaveLoadSystem.OnSaveDataLoaded += CheckQuestDataLoaded;
            } else
            {
                MoonBunnyLog.print("ProgressSaveData load Fail.", tag:"LoadingUI");
                ProgressText.DOText(_SAVEDATA + _DOTS + _FAILED, TWEEN_DURATION);
                ConfirmUI.Description.text = _CANNOT_FIND_SAVE_DATA;
                ConfirmUI.OnOpen += () =>
                {
                    LoadingScene.LoadScene(SceneName.Scenario, 3);
                };
                ConfirmUI.OnExit += () =>
                {
                    GameManager.instance.QuitGame();
                };
                ConfirmUI.Open();
            }
        }

        private void CheckQuestDataLoaded()
        {
            if (QuestManager.instance.SaveLoadSystem.DataIsLoaded)
            {
                MoonBunnyLog.print("QuestSaveData load Success.", tag:"LoadingUI");
                _loadDataSucceed = true;
                ProgressText.DOText(_SAVEDATA + _DOTS + _SUCCESS, TWEEN_DURATION);
                LoadingScene.LoadScene(SceneName.Scenario, 3);
            } else
            {
                MoonBunnyLog.print("QuestSaveData load Fail.", tag:"LoadingUI");
                ProgressText.DOText(_SAVEDATA + _DOTS + _FAILED, TWEEN_DURATION);
                ConfirmUI.Description.text = _CANNOT_FIND_SAVE_DATA;
                ConfirmUI.OnOpen += () =>
                {
                    LoadingScene.LoadScene(SceneName.Scenario, 3);
                };
                ConfirmUI.OnExit += () =>
                {
                    GameManager.instance.QuitGame();
                };
                ConfirmUI.Open();
            }
        }
    }
}