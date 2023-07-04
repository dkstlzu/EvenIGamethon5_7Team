using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(menuName = "Specs/Item")]
    public class ItemSpec : ScriptableObject
    {
        public int Score;
		public Sprite Sprite;
    }
}