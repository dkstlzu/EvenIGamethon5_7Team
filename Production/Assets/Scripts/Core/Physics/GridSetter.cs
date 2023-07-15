#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace MoonBunny
{
    public class GridSetter
    {
        private static string path = "GridSetting";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
        static void OnBeforeSceneLoadRuntimeMethod()
        {
            GridTransform.GridSetting = Resources.Load<GridSetting>(path);
        }

#if UNITY_EDITOR
        public static bool ShowGrid = false;

        [MenuItem("Dev/ShowGrid")]
        static void ShowGridToggle()
        {
            ShowGrid = !ShowGrid;

            if (ShowGrid)
            {
                SceneView.duringSceneGui += ShowGridGizmo;
            }
            else
            {
                SceneView.duringSceneGui -= ShowGridGizmo;
            }
        }

        static void ShowGridGizmo(SceneView sceneView)
        {
            Camera sceneViewCam = sceneView.camera;
            Vector3 sceneViewCamPosition = sceneViewCam.transform.position;
            float sceneViewCamSize = sceneViewCam.orthographicSize;

            float yMin = sceneViewCamPosition.y - sceneViewCamSize;
            float yMax = sceneViewCamPosition.y + sceneViewCamSize;
            float xMin = sceneViewCamPosition.x - sceneViewCamSize * sceneViewCam.aspect;
            float xMax = sceneViewCamPosition.x + sceneViewCamSize * sceneViewCam.aspect;

            float x = xMin;
            float y = yMin;

            Vector2 gridSize = GridTransform.GetGridSize();
            Color outlineColor = Color.cyan;
            outlineColor.a = 0.3f;
            
            while (x < xMax)
            {
                while (y < yMax)
                {
                    Vector2 center = GridTransform.Snap(new Vector2(x, y));

                    Handles.DrawSolidRectangleWithOutline(new Rect(center - gridSize/2, gridSize), Color.clear, outlineColor);

                    y += gridSize.y;
                }

                x += gridSize.x;
                y = yMin;
            }
        }
#endif
    }
}