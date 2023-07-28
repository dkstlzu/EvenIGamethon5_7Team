using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class BackgroundImageSetter : MonoBehaviour
    {
        private void Reset()
        {
            Image image;
            if (TryGetComponent(out image))
            {
                image.color = MoonBunnyColor.BackgroundColor;
            }
            
            DestroyImmediate(this);
        }
    }
}