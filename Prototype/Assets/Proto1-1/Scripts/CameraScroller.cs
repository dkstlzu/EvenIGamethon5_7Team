using System;
using UnityEngine;

namespace EvenI7.Proto1_1
{
    public class CameraScroller : MonoBehaviour
    {
        private GameObject _player;

        public float TargetYPosition;
        [Range(0, 1)] public float DampingSpeed;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            TargetYPosition = _player.transform.position.y;
            if (TargetYPosition < transform.position.y) return;
            
            transform.position = new Vector3(transform.position.x
                ,Mathf.Lerp(transform.position.y, TargetYPosition, DampingSpeed)
                ,transform.position.z);
        }
    }
}