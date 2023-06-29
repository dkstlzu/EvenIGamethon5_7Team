using System;
using UnityEngine;

namespace EvenI7.ProtoScreenSplit
{
    public class MoonBoundary : MonoBehaviour
    {
        public Moon Moon;

        private void OnTriggerEnter2D(Collider2D other)
        {
            ProtoScreenSplitCharacter character;
            
            if (other.TryGetComponent<ProtoScreenSplitCharacter>(out character))
            {
                Moon.Collect(character.FriendCharacter);
            }
        }
    }
}