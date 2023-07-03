using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace MoonBunny
{
    public class SceneName
    {
        public const string Loading = "0. Loading";
        public const string Start = "1. Start";
        public const string Stage1 = "2. Stage1";
        public const string Stage2 = "3. Stage2";
        public const string Stage3 = "4. Stage3";
        public const string Stage4 = "5. Stage4";
        public const string Stage5 = "6. Stage5";
        public const string StageChallenge = "7. Stage Challenge";

        public static string[] GetNames()
        {
            return new string[]
            {
                Loading, Start, Stage1, Stage2, Stage3, Stage4, Stage5, StageChallenge
            };
        }
    }
    public class LoadingScene : MonoBehaviour
    {
        public InputAction AnyInputAction;
        public List<Sprite> SplashImages;
        [Range(0, 10)] public float SplashInterval;
        public AnimationCurve SplashCurve;
        public int SplashNumber = 0;

        [SerializeField] private SpriteRenderer _renderer;

        private float _timer;
        
        private void Awake()
        {
            AnyInputAction.Enable();

            AnyInputAction.performed += SkipSlpash;
            _renderer.sprite = SplashImages[0];
        }

        private void OnDestroy()
        {
            AnyInputAction.performed -= SkipSlpash;
            AnyInputAction.Disable();
            AnyInputAction.Dispose();
        }

        private void Update()
        {
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
            SceneManager.LoadScene(SceneName.Start);
        }
    }
}