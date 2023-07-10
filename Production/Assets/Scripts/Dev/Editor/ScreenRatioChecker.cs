using System;
using UnityEditor;
using UnityEngine;

namespace MoonBunny.Dev.Editor
{
    public class ScreenRatioChecker : UnityEditor.Editor
    {
        private static bool _isRatioCheckOn = false;

        private static float _horizontalDivision;
        private static int _intHorizontalDivision => Mathf.RoundToInt(_horizontalDivision);
        private static float _heightPerBox;
        private static float _verticalBoxNumber;
        private static int _intVerticalBoxNumber => Mathf.RoundToInt(_verticalBoxNumber);
        
        
        [MenuItem("Dev/Screen Ratio Checker On")]
        public static void ScreenRatioCheckOn()
        {
            _isRatioCheckOn = true;
            SceneView.duringSceneGui += OnSceneGUI;
        }
        
        [MenuItem("Dev/Screen Ratio Checker On", true)]
        public static bool ScreenRatioCheckOnValidation()
        {
            return !_isRatioCheckOn;
        }
        
        [MenuItem("Dev/Screen Ratio Checker Off")]
        public static void ScreenRatioCheckOff()
        {
            _isRatioCheckOn = false;
            SceneView.duringSceneGui -= OnSceneGUI;
        }
                
        [MenuItem("Dev/Screen Ratio Checker Off", true)]
        public static bool ScreenRatioCheckOffValidation()
        {
            return _isRatioCheckOn;
        }

        private static float sliderRectWidth = 200;
        private static float labelRectWidth = 100;
        private static float rectHeight = 20;
        private static float rectSpaceX = 20;
        private static float rectSpaceY = 25;
        
        
        static private GUIStyle labelGUIStyle = new GUIStyle()
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 20,
        };

        
        static Rect horizontalDivisionSliderRect = new Rect(100, 50, sliderRectWidth, rectHeight);
        static Rect horizontalDivisionLabelRect => new Rect(horizontalDivisionSliderRect.xMax + rectSpaceX, horizontalDivisionSliderRect.y, labelRectWidth, rectHeight);
        private static Rect heightPerBoxSliderRect => new Rect(horizontalDivisionSliderRect.x, horizontalDivisionSliderRect.y + rectSpaceY,
            horizontalDivisionSliderRect.width, horizontalDivisionSliderRect.height);
        private static Rect heightPerBoxLabelRect => new Rect(heightPerBoxSliderRect.xMax + rectSpaceX, heightPerBoxSliderRect.y, labelRectWidth, rectHeight);
        private static Rect verticalBoxNumberRect => new Rect(heightPerBoxSliderRect.x, heightPerBoxSliderRect.y + rectSpaceY,
            heightPerBoxSliderRect.width, heightPerBoxSliderRect.height);
        private static Rect verticalBoxLabelRect => new Rect(verticalBoxNumberRect.xMax + rectSpaceX, verticalBoxNumberRect.y, labelRectWidth, rectHeight);

        private static float targetWidth = 1080;
        private static float targetHeight = 2340;
        private static float camHeight = Camera.main.orthographicSize * 2;
        private static float camWidth = camHeight * Camera.main.aspect;
        private static float pixelToUnit = targetHeight / camHeight;
        private static float unitHeightPerBox => _heightPerBox * pixelToUnit;
        private static float widthPerBox => targetWidth / _intHorizontalDivision;
        

        
        public static void OnSceneGUI(SceneView sceneView)
        {
            if (!_isRatioCheckOn) return;
            
            Handles.BeginGUI();
            _horizontalDivision = GUI.HorizontalSlider(horizontalDivisionSliderRect, _horizontalDivision, 0, 20);
            GUI.Label(horizontalDivisionLabelRect, _intHorizontalDivision.ToString(), labelGUIStyle);
            _heightPerBox = GUI.HorizontalSlider(heightPerBoxSliderRect, _heightPerBox, 0, 20);
            GUI.Label(heightPerBoxLabelRect, _heightPerBox.ToString(), labelGUIStyle);
            _verticalBoxNumber = GUI.HorizontalSlider(verticalBoxNumberRect, _verticalBoxNumber, 0, 1000);
            GUI.Label(verticalBoxLabelRect, _intVerticalBoxNumber.ToString(), labelGUIStyle);
            
            Handles.EndGUI();

            for (int i = 0; i < _intHorizontalDivision; i++)
            {
                for (int j = 0; j < _intVerticalBoxNumber; j++)
                {
                    Rect showRect = new Rect(widthPerBox * i, unitHeightPerBox * j, widthPerBox, unitHeightPerBox);
                    Color faceColor = ((i + j) % 2 == 0) ? Color.red : Color.blue;
                    
                    Handles.DrawSolidRectangleWithOutline(showRect, faceColor, Color.white);
                }
            }
        }
    }
}