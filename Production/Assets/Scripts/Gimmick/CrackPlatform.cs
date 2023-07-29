using UnityEngine;

namespace MoonBunny
{
    public class CrackPlatform : Gimmick
    {
        [SerializeField] private Animator _animator;
        private static readonly int Crack = Animator.StringToHash("Crack");

        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;

            _animator.SetTrigger(Crack);
            return true;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}