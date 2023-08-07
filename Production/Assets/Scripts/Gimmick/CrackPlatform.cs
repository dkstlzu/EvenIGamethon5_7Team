using System;
using UnityEngine;

namespace MoonBunny
{
    public class CrackPlatform : Gimmick
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private AudioClip _audioClip;
        private static readonly int Crack = Animator.StringToHash("Crack");

        private void Start()
        {
            _renderer.sprite = PreloadedResources.instance.CrackPlatformSpriteList[GameManager.instance.Stage.StageLevel];
            _animator.runtimeAnimatorController =
                PreloadedResources.instance.CrackPlatformAnimatorControllerList[GameManager.instance.Stage.StageLevel];
        }

        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;

            _animator.SetTrigger(Crack);
            SoundManager.instance.PlayClip(_audioClip);
            InvokeOnCollision = false;
            return true;
        }
    }
}