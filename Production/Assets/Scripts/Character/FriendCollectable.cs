using UnityEngine;

namespace MoonBunny
{
    public class FriendCollectable : Gimmick
    {
        public static AudioClip S_FriendCollectedAudioClip;
        public FriendName Name;
        [SerializeField] private AudioClip _audioClip;

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
            
            SoundManager.instance.PlayClip(_audioClip);
            GameManager.instance.Stage.CollectDict[Name]++;
        }
    }
}