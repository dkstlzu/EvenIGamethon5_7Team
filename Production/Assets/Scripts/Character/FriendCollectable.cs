using UnityEngine;

namespace MoonBunny
{
    public class FriendCollectable : Gimmick
    {
        public FriendName Name;
        [SerializeField] private AudioClip _audioClip;

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
            
            SoundManager.instance.PlayClip(_audioClip);
        }
    }
}