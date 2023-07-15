using System;
using UnityEngine;

namespace MoonBunny
{
    public class Bee : Obstacle
    {
        public float FlySpeed;
        [SerializeField] private MoonBunnyRigidbody _rigidbody;

        private void Start()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            _rigidbody.Move(new Vector2(FlySpeed, 0));
        }
    }
}