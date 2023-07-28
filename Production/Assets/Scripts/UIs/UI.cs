using System;
using DG.Tweening;
using UnityEngine;

namespace MoonBunny.UIs
{
    public class UI : MonoBehaviour
    {
        public CanvasGroup CanvasGroup;
        protected const float DEFAULT_FADE_DURATION = 1;
        protected virtual void Reset()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }
        
        public void Open(float duration = DEFAULT_FADE_DURATION)
        {
            FadeIn(CanvasGroup, duration);
        }

        public void OnExitButtonClicked(float duration = DEFAULT_FADE_DURATION)
        {
            FadeOut(CanvasGroup, duration);
        }

        protected void FadeIn(CanvasGroup cg, float duration = DEFAULT_FADE_DURATION)
        {
            cg.DOFade(1, duration);
            cg.blocksRaycasts = true;
        }

        protected void FadeOut(CanvasGroup cg, float duration = DEFAULT_FADE_DURATION)
        {
            cg.DOFade(0, duration);
            cg.blocksRaycasts = false;
        }
    }
}