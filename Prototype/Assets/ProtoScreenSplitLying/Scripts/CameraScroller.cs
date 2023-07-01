using System;
using UnityEngine;

namespace EvenI7.ProtoScreenSplitLying
{
    public class CameraScroller : MonoBehaviour
    {
        public RectTransform BoundaryTransform;
        
        public Transform TargetTransform;
        [Range(0, 1)] public float DampingSpeed;
        public Vector2 Offset;

        private Camera _camera;
        
        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            float targetX = Mathf.Lerp(transform.position.x, TargetTransform.position.x + Offset.x, DampingSpeed);
            float targetY = Mathf.Lerp(transform.position.y, TargetTransform.position.y + Offset.y, DampingSpeed);

            targetX = Mathf.Clamp(targetX, BoundaryTransform.position.x + BoundaryTransform.rect.xMin,
                BoundaryTransform.position.x + BoundaryTransform.rect.xMax);
            targetY = Mathf.Clamp(targetY, BoundaryTransform.position.y + BoundaryTransform.rect.yMin,
                BoundaryTransform.position.y + BoundaryTransform.rect.yMax);
            
            transform.position = new Vector3(targetX ,targetY,transform.position.z);
        }
    }
}