using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonBunny
{
    public static class SceneName
    {
        public const string Loading = "0. Loading";
        public const string Start = "1. Start";
        public const string Stage1 = "2. Stage1";
        public const string Stage2 = "3. Stage2";
        public const string Stage3 = "4. Stage3";
        public const string Stage4 = "5. Stage4";
        public const string Stage5 = "6. Stage5";
        public const string StageChallenge = "7. Stage Challenge";
    }
    public class LoadingScene : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene(SceneName.Start);
            Destroy(this);
        }
    }
}