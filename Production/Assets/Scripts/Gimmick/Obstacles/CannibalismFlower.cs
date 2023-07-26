using Cinemachine;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public class CannibalismFlower : Obstacle
    {
        public static GameObject S_EffectPrefab;
        
        [SerializeField] private Animator _animator;
        public int ClickNumberToEscape;
        [SerializeField] private int _currentRemainingNumberToEscape;
        public float CoolTime;
        private MoonBunnyRigidbody _rigidbody;
        [SerializeField] private Vector2Int _spittingVelocity;
        private bool _wasLeft;
        [SerializeField] private CinemachineImpulseSource _impulseSource;
        [SerializeField] private GameObject TapTextGo;

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);

            Instantiate(S_EffectPrefab, transform.position, Quaternion.identity);
            _rigidbody = with;
            _wasLeft = _rigidbody.isMovingToLeft;
            _rigidbody.Disable();
            _rigidbody.GetComponentInChildren<SpriteRenderer>().enabled = false;
            _rigidbody.GetComponent<Character>().isIgnoringFlip = true;
            TapTextGo.SetActive(true);
            
            _currentRemainingNumberToEscape = ClickNumberToEscape;

            GameManager.instance.Stage.UI.OnDirectionChangeButtonClicked += OnButtonClicked;
        }

        private void OnButtonClicked()
        {
            _currentRemainingNumberToEscape--;
            _impulseSource.GenerateImpulse();

            if (_currentRemainingNumberToEscape <= 0)
            {
                GameManager.instance.Stage.UI.OnDirectionChangeButtonClicked -= OnButtonClicked;
                _rigidbody.Enable();
                _rigidbody.GetComponentInChildren<SpriteRenderer>().enabled = true;
                _rigidbody.GetComponent<Character>().isIgnoringFlip = false;
                TapTextGo.SetActive(false);

                _renderer.color = new Color(1, 1, 1, 0.5f);
                InvokeOnCollision = false;
                
                CoroutineHelper.Delay(() =>
                {
                    _renderer.color = Color.white;
                    InvokeOnCollision = true;
                }, CoolTime);

                if (_wasLeft)
                {
                    _rigidbody.Move(_spittingVelocity);
                }
                else
                {
                    _rigidbody.Move(new Vector2Int(-_spittingVelocity.x, _spittingVelocity.y));
                }
            }
        }
    }
}