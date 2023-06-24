using System;
using UnityEngine;

namespace EvenI7.ProtoScreenSplit
{
    public class ScreenSplitCameraScroller : MonoBehaviour
    {
        private Transform _player;
        public RectTransform BoundaryTransform;
        
        public Transform TargetTransform;
        [Range(0, 1)] public float DampingSpeed;

        private Camera _camera;
        
        private void Start()
        {
            _player = GameObject.FindWithTag("Player").transform;
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            float targetY = Mathf.Lerp(transform.position.y, _player.position.y, DampingSpeed);

            targetY = Mathf.Clamp(targetY, BoundaryTransform.position.y + BoundaryTransform.rect.yMin + _camera.orthographicSize,
                BoundaryTransform.position.y + BoundaryTransform.rect.yMax - _camera.orthographicSize);
            transform.position = new Vector3(transform.position.x
                ,targetY
                ,transform.position.z);
        }


    }
}