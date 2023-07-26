using UnityEngine;
using UnityEngine.Events;

namespace MoonBunny
{
    public class GimmickInvoker : Gimmick
    {
        public Gimmick TargetGimmick;
        public bool UseUnityEventInsteadOfInvoke;
        public UnityEvent Events;

        public override bool Invoke(MoonBunnyRigidbody with)
        {
            if (!base.Invoke(with)) return false;
            
            if (!UseUnityEventInsteadOfInvoke)
            {
                return TargetGimmick.Invoke(with);
            }
            
            Events.Invoke();
            return true;
        }
    }
}