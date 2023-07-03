using UnityEngine;

namespace MoonBunny.UIs
{
    public class GameOverUI : MonoBehaviour
    {
        public GameObject ClearUIGO;
        public GameObject FailUIGO;
        
        public void ClearUI()
        {
            ClearUIGO.SetActive(true);
        }

        public void FailUI()
        {
            FailUIGO.SetActive(true);
        }
    }
}