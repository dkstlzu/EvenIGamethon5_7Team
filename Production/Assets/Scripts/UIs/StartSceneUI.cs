using System;
using dkstlzu.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class StartSceneUI : MonoBehaviour
    {
        public GameObject StageSelectUI;

        public Button Stage1Button;
        public Button Stage2Button;
        public Button Stage3Button;
        public Button Stage4Button;
        public Button Stage5Button;
        public Button StageChallengeButton;

        private GameManager _gameManager;
        
        private void Start()
        {
            _gameManager = GameManager.instance;

            if (!_gameManager.Stage1Clear) Stage2Button.interactable = false;
            if (!_gameManager.Stage2Clear) Stage3Button.interactable = false;
            if (!_gameManager.Stage3Clear) Stage4Button.interactable = false;
            if (!_gameManager.Stage4Clear) Stage5Button.interactable = false;
            if (!_gameManager.Stage5Clear) StageChallengeButton.interactable = false;
        }

        public void OnStartButtonClicked()
        {
            StageSelectUI.SetActive(true);
        }

        public void OnExitButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void OnStageButtonClicked(string name)
        {
            StageName stageName;
            if (!StageName.TryParse(name, true, out stageName))
            {
                Debug.LogError("StageName is wrong while stage button clicked");
                return;
            }

            SceneManager.LoadScene(StringValue.GetStringValue(stageName));
        }
    }
}