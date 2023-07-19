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
        Spider,
    }
    public class StageUIBuff : MonoBehaviour
    {
        public GameObject MagnetBuff;
        public GameObject SpiderDebuff;

        public void BuffOn(BuffType type, float duration = -1)
        {
            GameObject targetGo = null;
            switch (type)
            {
                case BuffType.Magnet:
                    targetGo = MagnetBuff;
                    break;
                case BuffType.Spider:
                    targetGo = SpiderDebuff;
                    break;
            }
            
            targetGo.SetActive(true);
            if (duration > 0)
            {
                Image image = targetGo.GetComponentInChildren<Image>(true);
                StartCoroutine(BuffTimer(image, duration));
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
            
            image.transform.parent.gameObject.SetActive(false);
        }
    }
}