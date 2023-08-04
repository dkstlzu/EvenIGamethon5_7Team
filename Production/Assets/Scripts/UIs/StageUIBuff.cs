using System;
using System.Collections;
using dkstlzu.Utility;
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
    
    public class StageUIBuff : UI
    {
        public GameObject MagnetBuff;
        public GameObject SpiderDebuff;

        protected override void Awake()
        {
            base.Awake();
            
            Magnet.OnMangetItemTaken += OnMagnet;
            Rocket.OnRocketItemTaken += OnRocket;
            SpiderWeb.OnSpiderwebObstacleTaken += OnSpider;
        }

        private void OnDestroy()
        {
            Magnet.OnMangetItemTaken -= OnMagnet;
            Rocket.OnRocketItemTaken -= OnRocket;
            SpiderWeb.OnSpiderwebObstacleTaken -= OnSpider;
        }

        void OnMagnet(float power, float duration)
        {
            BuffOn(BuffType.Magnet, duration);
        }

        void OnRocket(float upSpeed, float duration)
        {
            BuffOn(BuffType.Rocket, duration);
        }

        void OnSpider(float slow, float duration)
        {
            BuffOn(BuffType.Spider, duration);
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
                new BuffTimer(image, duration);
            }
        }

        class BuffTimer : IUpdatable
        {
            private Image _image;
            private float _time;
            private float _timer;

            private TimeUpdatable _timeUpdatable;
            public BuffTimer(Image image, float time)
            {
                _image = image;
                _time = time;
                _timer = time;

                _timeUpdatable = new TimeUpdatable(this, 1);
                UpdateManager.instance.Register(_timeUpdatable);
            }

            public void Update(float delta)
            {
                _timer -= delta;
        
                _image.fillAmount = _timer / _time;

                if (_timer <= 0)
                {
                    _image.transform.parent.gameObject.SetActive(false);
                    UpdateManager.instance.Unregister(_timeUpdatable);
                }
            }
        }
    }
}