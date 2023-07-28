using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoonBunny.UIs
{
    public class FuckLayoutGroup : MonoBehaviour
    {
        private void Awake()
        {
            if (TryGetComponent(out LayoutGroup layoutGroup))
            {
                Destroy(layoutGroup);
            }
            
            Destroy(this);
        }
    }
}