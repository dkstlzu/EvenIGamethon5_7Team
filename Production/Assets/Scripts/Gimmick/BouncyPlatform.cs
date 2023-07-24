using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace MoonBunny
{
    public class BouncyPlatform : Gimmick
    {
        public static AudioClip S_JumpAudioClip;
        private static readonly int BounceHash = Animator.StringToHash("Bounce");

        public bool Enabled = true;
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _renderer;
        public int JumpPower;
        public int UpMoveRange;
        public int DownMoveRange;
        public int LeftMoveRange;
        public int RightMoveRange;
        public bool UpFirst;
        public bool RightFirst;

        public int Index;
        public List<BouncyPlatform> Pattern1PlatformList;
        public List<BouncyPlatform> Pattern2PlatformList;
        public int CurrentPattern = 1;

        public float LoopCycleSpeed;

        private Vector3 _loopStartPosition;
        private Vector3 _loopForwardPosition;
        private Vector3 _loopBackwardPosition;
        private Vector2 _loopDelta;
        private Vector2 _currentLoopDelta;
        float multiplier;

        private bool _doLoop = false;
        
        protected void Start()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
#endif
            int bounceLevel = JumpPower == 3 ? -1 : JumpPower == 4 ? 0 : JumpPower == 5 ? 1 : 0;
            _animator.runtimeAnimatorController =
                PreloadedResources.instance.BouncyPlatformAnimatorControllerList[GameManager.instance.Stage.StageLevel - 1];
            _animator.SetInteger("BounceLevel", bounceLevel);
            _animator.SetTrigger("Set");
            _renderer.sprite = PreloadedResources.instance.BouncyPlatformSpriteList[(GameManager.instance.Stage.StageLevel - 1) * 3 + bounceLevel + 1];

            _loopStartPosition = transform.position;

            if (UpFirst && RightFirst)
            {
                _loopForwardPosition = GridTransform.ToReal(GridTransform.GridPosition + new Vector2Int(UpMoveRange, RightMoveRange));
                _loopBackwardPosition = GridTransform.ToReal(GridTransform.GridPosition + new Vector2Int(DownMoveRange, LeftMoveRange));
            } else if (UpFirst && !RightFirst)
            {
                _loopForwardPosition = GridTransform.ToReal(GridTransform.GridPosition + new Vector2Int(UpMoveRange, LeftMoveRange));
                _loopBackwardPosition = GridTransform.ToReal(GridTransform.GridPosition + new Vector2Int(DownMoveRange, RightMoveRange));
            } else if (!UpFirst && RightFirst)
            {
                _loopForwardPosition = GridTransform.ToReal(GridTransform.GridPosition + new Vector2Int(DownMoveRange, RightMoveRange));
                _loopBackwardPosition = GridTransform.ToReal(GridTransform.GridPosition + new Vector2Int(UpMoveRange, LeftMoveRange));
            } else if (!UpFirst && !RightFirst)
            {
                _loopForwardPosition = GridTransform.ToReal(GridTransform.GridPosition + new Vector2Int(DownMoveRange, LeftMoveRange));
                _loopBackwardPosition = GridTransform.ToReal(GridTransform.GridPosition + new Vector2Int(UpMoveRange, RightMoveRange));
            }

            _loopDelta = (_loopForwardPosition - _loopBackwardPosition).normalized * LoopCycleSpeed;
            _currentLoopDelta = _loopDelta;

            if ((UpMoveRange != 0 || DownMoveRange != 0 || LeftMoveRange != 0 || RightMoveRange != 0) && LoopCycleSpeed > 0)
            {
                _doLoop = true;
            }
        }

        protected void FixedUpdate()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
#endif
            
            if (!_doLoop) return;
            
            transform.Translate(_currentLoopDelta * Time.deltaTime);

            if (UpFirst)
            {
                // Forward
                if ((transform.position.y >= _loopForwardPosition.y))
                {
                    _currentLoopDelta = -_loopDelta;
                }
            } else
            {
                // Backward
                if (transform.position.y <= _loopForwardPosition.y)
                {
                    _currentLoopDelta = _loopDelta;
                }
            }
        }
        
        public override void Invoke(MoonBunnyRigidbody with)
        {
            if (!Enabled) return;
            
            base.Invoke(with);
            
            _animator.SetTrigger(BounceHash);
            SoundManager.instance.PlayClip(S_JumpAudioClip);

            if (CurrentPattern == 1)
            {
                CurrentPattern = 2;
                foreach (BouncyPlatform platform in Pattern1PlatformList)
                {
                    platform.Enabled = false;
                }

                foreach (BouncyPlatform platform in Pattern2PlatformList)
                {
                    platform.Enabled = true;
                }
            } else if (CurrentPattern == 2)
            {
                CurrentPattern = 1;
                foreach (BouncyPlatform platform in Pattern1PlatformList)
                {
                    platform.Enabled = true;
                }

                foreach (BouncyPlatform platform in Pattern2PlatformList)
                {
                    platform.Enabled = false;
                }
            }
        }
    }
}