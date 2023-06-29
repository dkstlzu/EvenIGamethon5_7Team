using dkstlzu.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonBunny.UIs
{
    public class StartSceneUI : MonoBehaviour
    {
        public GameObject StageSelectUI;
            
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