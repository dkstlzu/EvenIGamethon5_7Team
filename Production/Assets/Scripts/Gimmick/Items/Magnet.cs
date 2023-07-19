using dkstlzu.Utility;
using MoonBunny.UIs;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoonBunny
{
    public class Magnet : Item
    {
        public float MagnetPower;
        public float Duration;

        public override void Invoke(MoonBunnyRigidbody with)
        {
            base.Invoke(with);
            
            with.GetComponent<Character>().SetMagneticPower(MagnetPower, Duration);
            FindObjectOfType<StageUIBuff>().BuffOn(BuffType.Magnet, Duration);
            
            GetComponent<Collider2D>().enabled = false;
            _renderer.enabled = false;
            Destroy(gameObject, 2);
        }
    }
}