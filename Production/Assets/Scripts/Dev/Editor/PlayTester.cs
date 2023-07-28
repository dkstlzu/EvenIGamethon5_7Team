using MoonBunny;
using MoonBunny.Effects;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dev.Editor
{
    public class PlayTester
    {
        private static Character _character;
        private static string LevelPath = "Assets/Prefab/Level/";
        
        [MenuItem("Dev/PlayTest/Restart Stage")]
        public static void RestartStage()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        [MenuItem("Dev/PlayTest/HP Recovery")]
        public static void GetHeart()
        {
            _character.GetHeart();
            _character.GetHeart();
            _character.GetHeart();
        }

        [MenuItem("Dev/PlayTest/Magnet")]
        public static void ForceMagnet()
        {
            new MagnetEffect(GameObject.FindWithTag("Player").GetComponentInChildren<Character>(), 2, 10).Effect();
        }
        
        [MenuItem("Dev/PlayTest/Rocket")]
        public static void ForceRocket()
        {
            new RocketEffect(GameObject.FindWithTag("Player").GetComponent<MoonBunnyRigidbody>(), 3, 5).Effect();
        }
        
        [MenuItem("Dev/PlayTest/StarCandy")]
        public static void ForceStarCandy()
        {
            Bounds area = new Bounds(GameObject.FindWithTag("Player").transform.position, new Vector2(50, 6));
            new StarCandyEffect(LayerMask.GetMask("Obstacle"), area).Effect();
        }

        [MenuItem("Dev/PlayTest/Thunder")]
        public static void Thunder()
        {
            new ThunderEffect(1, 2).Effect();
        }

        [MenuItem("Dev/PlayTest/Restart Stage", true)]
        [MenuItem("Dev/PlayTest/HP Recovery", true)]
        [MenuItem("Dev/PlayTest/Magnet", true)]
        [MenuItem("Dev/PlayTest/Rocket", true)]
        [MenuItem("Dev/PlayTest/StarCandy", true)]
        [MenuItem("Dev/PlayTest/Thunder", true)]
        static bool PlayingValidation()
        {
            bool isPlaying = EditorApplication.isPlaying;
            bool isStage = SceneName.isStage(SceneManager.GetActiveScene().name);
            if (isPlaying && isStage)
            {
                _character = GameObject.FindWithTag("Player").GetComponent<Character>();
            }
            return isPlaying && isStage;
        }
    }
}