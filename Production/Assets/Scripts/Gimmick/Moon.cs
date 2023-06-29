using System;
using UnityEngine;

namespace MoonBunny
{
    public class Moon : MonoBehaviour
    {
        public event Action<Friend> OnFriendCharacterArrivedToMoon;

        public void Collect(Friend character)
        {
            character.Collect();
            OnFriendCharacterArrivedToMoon?.Invoke(character);
        }
    }
}