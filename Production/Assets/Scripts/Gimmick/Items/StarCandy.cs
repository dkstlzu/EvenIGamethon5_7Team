using UnityEngine;

namespace MoonBunny
{
    public class StarCandy : Item
    {
        public LayerMask TargetLayerMask;
        public Collider2D TargetArea;
        public int MaxDestroyNumber;
            
        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);

            ContactFilter2D filter = new ContactFilter2D();
            filter.layerMask = TargetLayerMask;
            filter.useLayerMask = true;

            Collider2D[] results = new Collider2D[MaxDestroyNumber];
            TargetArea.OverlapCollider(filter, results);
            
            for (int i = 0; i < results.Length; i++)
            {
                if (results[i] == null) continue;
                
                Destroy(results[i].gameObject);
            }
        }
    }
}