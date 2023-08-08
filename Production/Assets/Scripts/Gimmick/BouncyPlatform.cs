﻿using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace MoonBunny
{
    public class BouncyPlatform : Gimmick
    {
        public static Action OnInvoke;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void StaticEventInit()
        {
            OnInvoke = null;
        }
        
        
        public static AudioClip S_JumpAudioClip;
        private static readonly int BounceHash = Animator.StringToHash("Bounce");

        public bool Enabled = true;
        [SerializeField] private MoonBunnyRigidbody _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private SpriteRenderer _virtualRenderer;
        public bool isVirtual = false;
        public int JumpPower;
        public int VerticalMoveRange;
        public int HorizontalMoveRange;

        public int Index;
        public List<BouncyPlatform> Pattern1PlatformList;
        public List<BouncyPlatform> Pattern2PlatformList;
        public int CurrentPattern = 1;

        public float LoopCycleSpeed;

        private Vector3 _loopStartPosition;
        private Vector3 _loopForwardPosition;
        private Vector2 _loopDelta;
        private Vector2 _currentLoopDelta;
        float multiplier;

        private int _invokeNumberToCrack = 5;
        private int _currentInvokeNumber = 0;

        private bool _doLoop = false;
        
        protected void Start()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
#endif
            int bounceLevel = JumpPower == 3 ? 0 : JumpPower == 4 ? 1 : JumpPower == 5 ? 2 : 1;
            _animator.runtimeAnimatorController =
                PreloadedResources.instance.BouncyPlatformAnimatorControllerList[GameManager.instance.Stage.StageLevel];
            _animator.SetInteger("BounceLevel", bounceLevel-1);
            _animator.SetTrigger("Set");
            _renderer.sprite = PreloadedResources.instance.BouncyPlatformSpriteList[(GameManager.instance.Stage.StageLevel) * 3 + bounceLevel];

            _loopStartPosition = transform.position;
            _loopForwardPosition = GridTransform.ToReal(GridTransform.ToGrid(_loopStartPosition) + new Vector2Int(HorizontalMoveRange, VerticalMoveRange));

            _loopDelta = (_loopForwardPosition - _loopStartPosition).normalized * LoopCycleSpeed;
            _currentLoopDelta = _loopDelta;
            
            if ((VerticalMoveRange != 0 || HorizontalMoveRange != 0) && LoopCycleSpeed > 0)
            {
                _doLoop = true;
            }

            _rigidbody.DisableCollision();
            
            CheckMovement();
        }

        protected void FixedUpdate()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
#endif
            if (_doLoop)
            {
                // Forward
                if ((_loopDelta.x > 0 && transform.position.x > _loopForwardPosition.x) || (_loopDelta.x < 0 && transform.position.x < _loopForwardPosition.x))
                {
                    _currentLoopDelta = -_loopDelta;
                    CheckMovement();
                } else if ((_loopDelta.x > 0 && transform.position.x < _loopStartPosition.x) || (_loopDelta.x < 0 && transform.position.x > _loopStartPosition.x))
                {
                    _currentLoopDelta = _loopDelta;
                    CheckMovement();
                }
                
                if ((_loopDelta.y > 0 && transform.position.y > _loopForwardPosition.y) || (_loopDelta.y < 0 && transform.position.y < _loopForwardPosition.y))
                {
                    _currentLoopDelta = -_loopDelta;
                    CheckMovement();
                } else if ((_loopDelta.y > 0 && transform.position.y < _loopStartPosition.y) || (_loopDelta.y < 0 && transform.position.y > _loopStartPosition.y))
                {
                    _currentLoopDelta = _loopDelta;
                    CheckMovement();
                }
            }
        }
        
        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!Enabled) return false;
            
            if (!base.Invoke(with, direction)) return false;

            _animator.SetTrigger(BounceHash);
            SoundManager.instance.PlayClip(S_JumpAudioClip);

            if (CurrentPattern == 1)
            {
                CurrentPattern = 2;
                foreach (BouncyPlatform platform in Pattern1PlatformList)
                {
                    if (platform) platform.MakeVirtual();
                }

                foreach (BouncyPlatform platform in Pattern2PlatformList)
                {
                    if (platform) platform.MakeConcrete();
                }
            } else if (CurrentPattern == 2)
            {
                CurrentPattern = 1;
                foreach (BouncyPlatform platform in Pattern1PlatformList)
                {
                    if (platform) platform.MakeConcrete();
                }

                foreach (BouncyPlatform platform in Pattern2PlatformList)
                {
                    if (platform) platform.MakeVirtual();
                }
            }
            
            _currentInvokeNumber++;

            if (_currentInvokeNumber >= _invokeNumberToCrack)
            {
                Destroy(gameObject);
            }

            return true;
        }

        public void MakeVirtual()
        {
            isVirtual = true;
            _virtualRenderer.enabled = true;
            _renderer.enabled = false;
            InvokeOnCollision = false;
            CheckMovement();
        }

        public void MakeConcrete()
        {
            isVirtual = false;
            _virtualRenderer.enabled = false;
            _renderer.enabled = true;
            InvokeOnCollision = true;
            CheckMovement();
        }

        public void CheckMovement()
        {
            if (isVirtual)
            {
                _rigidbody.StopMove();
            }
            else
            {
                _rigidbody.Move(_currentLoopDelta);
            }
        }

        public override Collision[] Collide(MoonBunnyRigidbody rigidbody, MoonBunnyCollider.Direction direction)
        {
            if (!InvokeOnCollision) return Array.Empty<Collision>();

            List<Collision> collisions = new List<Collision>();
            
            if (rigidbody.GridObject is Character character)
            {
                collisions.Add(new BouncyPlatformCollision(rigidbody, this));
            }
            
            collisions.AddRange(base.Collide(rigidbody, direction));

            return collisions.ToArray();
        }
    }
}
