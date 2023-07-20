using UnityEngine;

namespace MoonBunny
{
    public class CrackPlatform : Gimmick
    {
        [SerializeField] private Animator _animator;
        private static readonly int Crack = Animator.StringToHash("Crack");

        public override void Invoke(MoonBunnyRigidbody rigidbody)
        {
            base.Invoke(rigidbody);
            
            _animator.SetTrigger(Crack);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}