using System;
using Cinemachine;
using UnityEngine;

namespace MoonBunny
{
    public class CameraSetter : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private CinemachineConfiner2D _confiner2D;

        protected void Start()
        {
            _confiner2D.m_BoundingShape2D = GameManager.instance.Stage.LevelConfiner;
            _confiner2D.InvalidateCache();
        }
    }
}