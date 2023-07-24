using Cinemachine;
using UnityEngine;

namespace MoonBunny
{
    public class CannibalismFlower : Obstacle
    {
        public static GameObject S_EffectPrefab;
        
        [SerializeField] private Animator _animator;
        public int ClickNumberToEscape;
        [SerializeField] private int _currentRemainingNumberToEscape;
        private MoonBunnyRigidbody _rigidbody;
        [SerializeField] private Vector2Int _spittingVelocity;
        private bool _wasLeft;
        [SerializeField] private CinemachineImpulseSource _impulseSource;

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);

            Instantiate(S_EffectPrefab, transform.position, Quaternion.identity);
            _rigidbody = with;
            _wasLeft = _rigidbody.isMovingToLeft;
            _rigidbody.Disable();
            _rigidbody.GetComponentInChildren<SpriteRenderer>().enabled = false;
            _rigidbody.GetComponent<Character>().isCanniblismEaten = true;

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
                _rigidbody.GetComponent<Character>().isCanniblismEaten = false;
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