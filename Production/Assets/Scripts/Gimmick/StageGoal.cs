using System;
using UnityEngine;

namespace MoonBunny
{
    public class StageGoal : Gimmick
    {
        public Stage Stage;
        [SerializeField] private SpriteRenderer _renderer;

        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;

            Stage.Clear();
            return true;
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying) return;
#endif

            _renderer.sprite = PreloadedResources.instance.LevelEndPlatformSpriteList[Stage.StageLevel];
        }
    }
}