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
        Rocket,
        Spider,
    }
    
    public class StageUIBuff : MonoBehaviour
    {
        public GameObject MagnetBuff;
        public GameObject SpiderDebuff;

        private void Awake()
        {
            Magnet.OnMangetItemTaken += (power, duration) => BuffOn(BuffType.Magnet, duration);
            Rocket.OnRocketItemTaken += (upSpeed, duration) => BuffOn(BuffType.Rocket, duration);
            SpiderWeb.OnSpiderwebObstacleTaken += (slow, duration) => BuffOn(BuffType.Spider, duration);
        }

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

            if (targetGo == null) return;
            
            targetGo.SetActive(true);
            if (duration > 0)
            {
                Image image = targetGo.GetComponentInChildren<Image>(true);
                StartCoroutine(BuffTimer(image, duration));
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