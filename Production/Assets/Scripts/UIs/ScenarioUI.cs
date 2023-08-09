using System;
using dkstlzu.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace MoonBunny.UIs
{
    public class ScenarioUI : UI
    {
        public Animator IntroAnimator;
        public GameObject Board;

        private GameManager _gameManager;
        public InputAction AnyKeyInputAction;

        private void OnDestroy()
        {
            AnyKeyInputAction.Dispose();
            AnyKeyInputAction.Disable();
        }

        public void OnSplashFinish()
        {
            Board.SetActive(true);
            IntroAnimator.enabled = true;
            
            CoroutineHelper.Delay(() =>
            {
                AnyKeyInputAction.Enable();
                AnyKeyInputAction.performed += OnPressTheAnyKeyIntro;
            }, 0.5f);
        }

        void OnPressTheAnyKeyIntro(InputAction.CallbackContext context)
        {
            var state = IntroAnimator.GetCurrentAnimatorStateInfo(0);
            if (state.normalizedTime >= 1)
            {

                SceneManager.LoadSceneAsync(SceneName.Start);
            } else if (state.normalizedTime >= 0.9f)
            {
                
            }
            else
            {
                IntroAnimator.Play("IntroSceneAnimation", 0, 0.9f);
            }
        }

        public void OnAfterIntroFinish()
        {
            IntroAnimator.enabled = false;
        }
    }
}