// using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class LoadingUI : UI
    {
        public TextMeshProUGUI Text;
        public Button Button;

        public void OnLoginButtonClicked()
        {
            // GoogleManager.instance.Login(SignInInteractivity.CanPromptAlways);
        }
    }
}