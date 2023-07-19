using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace MoonBunny
{
    public class BouncyPlatform : Platform
    {
        private static readonly int BounceHash = Animator.StringToHash("Bounce");

        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _renderer;
        public int JumpPower;
        public int VerticalMoveRange;
        public int HorizontalMoveRange;

        public float LoopCycleLength;

        private Vector3 _loopStartPosition;
        private Vector3 _loopEndPosition;
        private Vector3 _loopCenterPosition;
        private float _loopXDelta;
        private float _loopYDelta;
        float multiplier;

        private float _timer;
        private bool _doLoop = false;
        
        protected void Start()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
#endif
            _renderer.sprite = PreloadedResources.instance.BouncyPlatformSpriteList[GameManager.instance.Stage.StageLevel - 1];
            _animator.runtimeAnimatorController =
                PreloadedResources.instance.BouncyPlatformAnimatorControllerList[GameManager.instance.Stage.StageLevel - 1];

            Vector2Int loopEndPosition = GridTransform.GridPosition + new Vector2Int(HorizontalMoveRange, VerticalMoveRange);

            _loopStartPosition = transform.position;
            _loopEndPosition = GridTransform.ToReal(loopEndPosition);
            _loopCenterPosition = (_loopStartPosition + _loopEndPosition) / 2;
            _loopXDelta = (_loopEndPosition.x - _loopStartPosition.x) / 2;
            _loopYDelta = (_loopEndPosition.y - _loopStartPosition.y) / 2;
            multiplier = 2 * Mathf.PI / LoopCycleLength;

            if ((HorizontalMoveRange != 0 || VerticalMoveRange != 0) && LoopCycleLength > 0)
            {
                _doLoop = true;
            }
        }

        protected override void Update()
        {
            base.Update();
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
#endif
            
            if (!_doLoop) return;
            
            _timer += Time.deltaTime;

            float x = _loopXDelta * Mathf.Sin(_timer * multiplier);
            float y = _loopYDelta * Mathf.Sin(_timer * multiplier);

            transform.position = new Vector3(x, y, transform.position.z) + _loopCenterPosition;
        }
        
        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
            
            _animator.SetTrigger(BounceHash);
        }
    }
}