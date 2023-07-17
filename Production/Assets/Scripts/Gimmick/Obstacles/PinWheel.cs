using UnityEngine;

namespace MoonBunny
{
    public class PinWheel : Obstacle
    {
        [SerializeField] private Animator _animator;
        private static readonly int Wheel = Animator.StringToHash("PinWheel");

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
            
            _animator.SetTrigger(Wheel);
        }
    }
}