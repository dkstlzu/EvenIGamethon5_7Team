using UnityEngine;
using UnityEngine.Events;

namespace MoonBunny
{
    public class GimmickInvoker : Gimmick
    {
        public Gimmick TargetGimmick;
        public bool UseUnityEventInsteadOfInvoke;
        public UnityEvent Events;

        public override bool Invoke(MoonBunnyRigidbody with, MoonBunnyCollider.Direction direction)
        {
            if (!base.Invoke(with, direction)) return false;
            
            if (!UseUnityEventInsteadOfInvoke)
            {
                return TargetGimmick.Invoke(with, direction);
            }
            
            Events.Invoke();
            return true;
        }
    }
}