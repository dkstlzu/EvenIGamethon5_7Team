using System;
using Cinemachine;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public class CameraSetter : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private CinemachineConfiner2D _confiner2D;

        public event Action OnCameraSetFinish;
        public float CameraSetTime;
        
        protected void Start()
        {
            AudioListener.volume = GameManager.instance.VolumeSetting;
            _confiner2D.m_BoundingShape2D = GameManager.instance.Stage.LevelConfiner;
            _confiner2D.InvalidateCache();
            
            CoroutineHelper.Delay(() =>
            {
                OnCameraSetFinish?.Invoke();
                if (GameManager.instance.ShowTutorial)
                {
                    GameManager.instance.Stage.UI.TutorialOn();
                }
                else
                {
                    GameManager.instance.Stage.UI.GetComponent<Animator>().enabled = true;
                }
            }, CameraSetTime);
        }
    }
}