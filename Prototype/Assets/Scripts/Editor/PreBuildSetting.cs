using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EvenI7
{
    public class PreBuildSetting : IPreprocessBuildWithReport
    {
        private int _order = 1;
        public int callbackOrder => _order;
        
        public void OnPreprocessBuild(BuildReport report)
        {
            Debug.Log("BuildTest");
            if (SceneManager.GetActiveScene().name == "ProtoScreenSplitLying")
            {
                Debug.Log("BuildTest1");
                Physics2D.gravity = new Vector2(0, -4);
            }
            else
            {
                Debug.Log("BuildTest2");
                Physics2D.gravity = new Vector2(0, -10);
            }

            if (SceneManager.GetActiveScene().name == "ProtoScreenSplitVertical")
            {
                Debug.Log("BuildTest3");
                PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            }
            else
            {
                Debug.Log("BuildTest4");
                PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
                PlayerSettings.allowedAutorotateToLandscapeLeft = true;
                PlayerSettings.allowedAutorotateToLandscapeRight = true;
                PlayerSettings.allowedAutorotateToPortrait = false;
                PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
            }
            
        }
    }
}