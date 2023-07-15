using UnityEngine;
using UnityEngine.Events;

namespace MoonBunny
{
    public class GimmickInvoker : Gimmick
    {
        public Gimmick TargetGimmick;
        public bool UseUnityEventInsteadOfInvoke;
        public UnityEvent Events;

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);

            if (!UseUnityEventInsteadOfInvoke)
            {
                TargetGimmick.Invoke(with);
            }
            
            Events.Invoke();
        }
    }
}