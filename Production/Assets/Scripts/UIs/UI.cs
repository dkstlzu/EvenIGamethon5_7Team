using System;
using DG.Tweening;
using dkstlzu.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class UI : MonoBehaviour
    {
        public static void SetColorOfChildren(RectTransform rectTransform, Color color)
        {
            var graphics = rectTransform.GetComponentsInChildren<Graphic>();

            foreach (var graphic in graphics)
            {
                graphic.color = color;
            }
        }
            
        public CanvasGroup CanvasGroup;
        protected const float DEFAULT_FADE_DURATION = 0.2f;

        protected bool _isFading = false;

        public event Action OnOpen;
        public event Action OnExit;
        
        protected virtual void Reset()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }

        public void Open()
        {
            Open(DEFAULT_FADE_DURATION);
        }
        
        public void Open(float duration)
        {
            if (_isFading) return;
            
            FadeIn(CanvasGroup, duration);
            OnOpen?.Invoke();
        }

        public void OnExitButtonClicked()
        {
            OnExitButtonClicked(DEFAULT_FADE_DURATION);
        }

        public void OnExitButtonClicked(float duration)
        {
            if (_isFading) return;
            
            FadeOut(CanvasGroup, duration);
            OnExit?.Invoke();
        }

        protected void FadeIn(CanvasGroup cg, float duration = DEFAULT_FADE_DURATION)
        {
            _isFading = true;
            UpdateManager.instance.Delay(() => _isFading = false, duration);
            cg.DOFade(1, duration);
            cg.blocksRaycasts = true;
        }

        protected void FadeOut(CanvasGroup cg, float duration = DEFAULT_FADE_DURATION)
        {
            _isFading = true;
            UpdateManager.instance.Delay(() => _isFading = false, duration);
            cg.DOFade(0, duration);
            cg.blocksRaycasts = false;
        }
    }
}