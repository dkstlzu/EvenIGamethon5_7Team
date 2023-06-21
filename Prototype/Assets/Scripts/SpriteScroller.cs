using System;
using UnityEngine;

namespace EvenI7
{
    public class SpriteScroller : MonoBehaviour
    {
        public SpriteRenderer Renderer;
        [SerializeField] private Vector2 offset;

        private void Update()
        {
            offset.x += Time.deltaTime;
            Renderer.material.mainTextureOffset = offset;
        }
    }
}