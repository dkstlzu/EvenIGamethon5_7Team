using dkstlzu.Utility;
using MoonBunny.Effects;
using UnityEngine;

namespace MoonBunny
{
    public class Buff : IEffect
    {
        protected Character _character;
        protected MoonBunnyRigidbody _rigidbody;

        public Buff(Character character, MoonBunnyRigidbody rigidbody)
        {
            _character = character;
            _rigidbody = rigidbody;
        }
        
        public virtual void Apply()
        {
            _character.BuffList.Add(this);
        }

        public virtual void Remove()
        {
            _character.BuffList.Remove(this);
        }

        public virtual void Effect()
        {
        }
    }

    public class DurationBuff : Buff
    {
        protected float _duration;
        
        public DurationBuff(float duration, Character character, MoonBunnyRigidbody rigidbody) : base(character, rigidbody)
        {
            _duration = duration;
        }

        public override void Apply()
        {
            base.Apply();
            
            CoroutineHelper.Delay(Remove, _duration);
        }
    }

}