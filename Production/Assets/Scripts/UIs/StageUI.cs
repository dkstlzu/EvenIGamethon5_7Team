using System;
using UnityEngine;

namespace MoonBunny.UIs
{
    public class StageUI : MonoBehaviour
    {
        public GameObject PauseUI;

        private void Start()
        {
#if UNITY_EDITOR
            GameObject.FindWithTag("GameController").GetComponent<InputManager>().OnESCInputPerformed += Pause;
#endif
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            GameObject.FindWithTag("GameController").GetComponent<InputManager>().OnESCInputPerformed -= Pause;
#endif
        }

        public void Pause()
        {
            PauseUI.SetActive(true);
        }
    }
}