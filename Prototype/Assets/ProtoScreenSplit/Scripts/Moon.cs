using System;
using UnityEngine;

namespace EvenI7.ProtoScreenSplit
{
    public class Moon : MonoBehaviour
    {
        public event Action<FriendCharacter> OnFriendCharacterArrivedToMoon;

        public void Collect(FriendCharacter character)
        {
            character.Collect();
            OnFriendCharacterArrivedToMoon?.Invoke(character);
        }
    }
}