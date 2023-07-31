using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace MoonBunny.Dev.Editor
{
    public class BuildKeyStore : IPreprocessBuildWithReport
    {
        private const string KEYPATH = "evenbunnycute13579";
        private const string KEYPASSWORD = "evenbunnycute13579";
        
        private int _callbackOrder;
        public int callbackOrder => _callbackOrder;
        public void OnPreprocessBuild(BuildReport report)
        {
            PlayerSettings.Android.keyaliasPass = KEYPATH;
            PlayerSettings.Android.keystorePass = KEYPASSWORD;

            switch (report.summary.platform)
            {
                case BuildTarget.Android:
                    PlayerSettings.Android.bundleVersionCode++;
                    break;
            }
        }

        public static void BuildMono()
        {

        }

        private static string GetBuildName()
        {
            return $"{PlayerSettings.productName}_{PlayerSettings.bundleVersion}.apk";
        }
    }
}