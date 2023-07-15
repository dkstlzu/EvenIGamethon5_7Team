using UnityEngine;

namespace dkstlzu.Utility
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<T>();
                }

                return _instance;
            }
        }

        public static T CreateIfNull()
        {
            if (_instance == null)
            {
                _instance = new GameObject($"{typeof(T)} instance").AddComponent<T>();
            }

            return _instance;
        }
        
    }
}