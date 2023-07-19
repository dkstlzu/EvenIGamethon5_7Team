using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public enum BuffType
    {
        None,
        Magnet,
    }
    public class StageUIBuff : MonoBehaviour
    {
        public GameObject MagnetBuff;

        public void BuffOn(BuffType type, float duration = -1)
        {
            GameObject targetGo = null;
            switch (type)
            {
                case BuffType.Magnet:
                    targetGo = MagnetBuff;
                    break;
            }
            
            targetGo.SetActive(true);
            if (duration > 0)
            {
                StartCoroutine(BuffTimer(targetGo.GetComponentInChildren<Image>(), duration));
            }
        }

        public void BuffOff(BuffType type)
        {
            switch (type)
            {
                case BuffType.Magnet:
                    MagnetBuff.SetActive(false);
                    break;
            }
        }

        IEnumerator BuffTimer(Image image, float time)
        {
            float _timer = time;

            while (_timer > 0)
            {
                _timer -= Time.deltaTime;

                image.fillAmount = _timer / time;

                yield return new WaitForEndOfFrame();
            }
            
            image.gameObject.SetActive(false);
        }
    }
}