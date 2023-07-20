using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace MoonBunny
{
    public static class SceneName
    {
        public const string Loading = "0. Loading";
        public const string Start = "1. Start";
        public const string Stage1_1 = "2. Stage1_1";
        public const string Stage1_2 = "2. Stage1_2";
        public const string Stage1_3 = "2. Stage1_3";
        public const string Stage2_1 = "3. Stage2_1";
        public const string Stage2_2 = "3. Stage2_2";
        public const string Stage2_3 = "3. Stage2_3";
        public const string Stage3_1 = "4. Stage3_1";
        public const string Stage3_2 = "4. Stage3_2";
        public const string Stage3_3 = "4. Stage3_3";
        public const string Stage4_1 = "5. Stage4_1";
        public const string Stage4_2 = "5. Stage4_2";
        public const string Stage4_3 = "5. Stage4_3";
        public const string Stage5_1 = "6. Stage5_1";
        public const string Stage5_2 = "6. Stage5_2";
        public const string Stage5_3 = "6. Stage5_3";
        public const string StageChallenge = "7. Stage Challenge";

        public static string[] Names = {Loading, Start, Stage1_1, Stage1_2, Stage1_3, Stage2_1, Stage2_2, Stage2_3, Stage3_1, Stage3_2, Stage3_3, Stage4_1, Stage4_2, Stage4_3, Stage5_1, Stage5_2, Stage5_3, StageChallenge};
        public static string[] StageNames = {Stage1_1, Stage1_2, Stage1_3, Stage2_1, Stage2_2, Stage2_3, Stage3_1, Stage3_2, Stage3_3, Stage4_1, Stage4_2, Stage4_3, Stage5_1, Stage5_2, Stage5_3, StageChallenge};

        public static bool isStage(string name)
        {
            return StageNames.Contains(name);
        }
    }
    public class LoadingScene : MonoBehaviour
    {
        public InputAction AnyInputAction;
        public List<Sprite> SplashImages;
        [Range(0, 10)] public float SplashInterval;
        public AnimationCurve SplashCurve;
        public int SplashNumber = 0;
        public int NextScene = -1;

        [SerializeField] private SpriteRenderer _renderer;

        private float _timer;
        
        private void Awake()
        {
            AnyInputAction.Enable();

            AnyInputAction.performed += SkipSlpash;
            if (SplashImages.Count > 0)
            {
                _renderer.sprite = SplashImages[0];
            }
        }

        private void OnDestroy()
        {
            AnyInputAction.performed -= SkipSlpash;
            AnyInputAction.Disable();
            AnyInputAction.Dispose();
        }

        private void Update()
        {
            if (SplashImages.Count == 0)
            {
                FinishSplash();
                return;
            }
            
            _timer += Time.deltaTime;

            if (_timer >= SplashInterval)
            {
                SplashNext();
            }

            _renderer.color = new Color(1, 1, 1, SplashCurve.Evaluate(_timer / SplashInterval));
        }

        void SplashNext()
        {
            SplashNumber++;

            if (SplashNumber >= SplashImages.Count)
            {
                FinishSplash();
                return;
            }
            

            _renderer.sprite = SplashImages[SplashNumber];
            _timer = 0;
        }
        
        private void SkipSlpash(InputAction.CallbackContext obj)
        {
            SplashNext();
        }

        void FinishSplash()
        {
            if (NextScene < 0)
            {
                Destroy(_renderer);
                Destroy(this);
            }
            else
            {
                SceneManager.LoadScene(SceneName.Names[NextScene]);
            }
        }
    }
}