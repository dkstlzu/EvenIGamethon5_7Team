using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    [DefaultExecutionOrder(-10)]
    public class PreloadedResources : Singleton<PreloadedResources>
    {
        public List<Sprite> BouncyPlatformSpriteList;
        public List<RuntimeAnimatorController> BouncyPlatformAnimatorControllerList;
        public AudioClip OpenStageAudioClip;

        [ContextMenu("Manually Load Assets")]
        private void Awake()
        {
            /*// Friend
            string[] friendNames = EnumHelper.ClapNamesOfEnum<FriendName>(0);
            string friendSpritePath = "Sprites/Characters/";

            for (int i = 0; i < friendNames.Length; i++)
            {
                Sprite sprite  = Resources.Load<Sprite>(Path.Combine(friendSpritePath, friendNames[i]));
                FriendSpriteList.Add(sprite);
            }

            string friendSpecPath = "Specs/Friend/";
            
            for (int i = 0; i < friendNames.Length; i++)
            {
                FriendSpec spec  = Resources.Load<FriendSpec>(Path.Combine(friendSpecPath, friendNames[i]));
                FriendSpecList.Add(spec);
            }
            
            // Item
            string[] itemNames = EnumHelper.ClapNamesOfEnum<ItemType>(0);
            string itemSpritePath = "Sprites/Items/";

            for (int i = 0; i < itemNames.Length; i++)
            {
                Sprite sprite = Resources.Load<Sprite>(Path.Combine(itemSpritePath, itemNames[i]));
                ItemSpriteList.Add(sprite);
            }

            string itemSpecPath = "Specs/Item/";

            for (int i = 0; i < itemNames.Length; i++)
            {
                ItemSpec spec = Resources.Load<ItemSpec>(Path.Combine(itemSpecPath, itemNames[i]));
                ItemSpecList.Add(spec);
            }
            
            // Obstacle
            string[] obstacleNames = EnumHelper.ClapNamesOfEnum<ObstacleType>(0);
            string obstacleSpritePath = "Sprites/Obstacles/";

            for (int i = 0; i < obstacleNames.Length; i++)
            {
                Sprite sprite = Resources.Load<Sprite>(Path.Combine(obstacleSpritePath, obstacleNames[i]));
                ObstacleSpriteList.Add(sprite);
            }

            string obstacleSpecPath = "Specs/Obstacles/";

            for (int i = 0; i < obstacleNames.Length; i++)
            {
                ObstacleSpec spec = Resources.Load<ObstacleSpec>(Path.Combine(obstacleSpecPath, obstacleNames[i]));
                ObstacleSpecList.Add(spec);
            }*/
        }

    }
}