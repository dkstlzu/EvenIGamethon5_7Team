using UnityEngine;

namespace EvenI7.ProtoScreenSplit
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
                    if (_instance == null)
                    {
                        GameObject go = new GameObject($"{typeof(T)} singleton instance");
                        _instance = go.AddComponent<T>(); 
                    }
                }

                return _instance;
            }
        }
    }
}