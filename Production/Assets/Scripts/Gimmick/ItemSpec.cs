using UnityEngine;

namespace MoonBunny
{
    [CreateAssetMenu(fileName = "_ItemSpec", menuName = "Specs/ItemSpec", order = 0)]
    public class ItemSpec : ScriptableObject
    {
        public int Score;
		public Sprite Sprite;
    }
}