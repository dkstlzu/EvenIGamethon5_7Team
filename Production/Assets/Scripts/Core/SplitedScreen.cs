using System;
using UnityEngine;

namespace MoonBunny
{
    public enum ScreenSplitAnimationType
    {
        FullToLeft,
        FullToMiddle,
        FullToRight,
        FullToLeftHalf,
        FullToRightHalf,
        LeftHalfToLeft,
        RightHalfToRight,
        LeftHalfToMiddle,
        RightHalfToMiddle,
        LeftToFull,
        LeftToLeftHalf,
        MiddleTofull,
        MiddleToLeftHalf,
        MiddleToRightHalf,
        RightToFull,
        RightToRightHalf,
        LeftHalfToFull,
        RightHalfToFull,
        VanishFromFull,
        VanishFromLeft,
        VanishFromMiddle,
        VanishFromRight,
        VanishFromLeftHalf,
        VanishFromRightHalf,
        EmptyToLeft,
        EmptyToMiddle,
        EmptyToRight,
        EmptyToLeftHalf,
        EmptyToRightHalf,
    }
    public class SplitedScreen : MonoBehaviour
    {
        public Character Character;
        public Camera Camera;
        public ScreenSplit.ScreenSide Side;

        public void Split(ScreenSplitAnimationType animationType, FriendName friendName = FriendName.None)
        {
            print($"{name} SplitedScreen split {animationType}");

            switch (animationType)
            {
                case ScreenSplitAnimationType.MiddleTofull:
                case ScreenSplitAnimationType.LeftToFull:
                case ScreenSplitAnimationType.RightToFull:
                case ScreenSplitAnimationType.LeftHalfToFull:
                case ScreenSplitAnimationType.RightHalfToFull:
                    Camera.depth = 2;
                    Camera.orthographicSize = 7;
                    Side = ScreenSplit.ScreenSide.MIDDLE;
                    break;
                
                case ScreenSplitAnimationType.EmptyToLeftHalf:
                case ScreenSplitAnimationType.FullToLeftHalf:
                    Camera.depth = 1;
                    Camera.orthographicSize = 11;
                    Side = ScreenSplit.ScreenSide.LEFT;
                    break;
                case ScreenSplitAnimationType.EmptyToRightHalf:
                case ScreenSplitAnimationType.FullToRightHalf:
                    Camera.depth = 1;
                    Camera.orthographicSize = 11;
                    Side = ScreenSplit.ScreenSide.RIGHT;
                    break;
                
                case ScreenSplitAnimationType.EmptyToLeft:
                case ScreenSplitAnimationType.FullToLeft:
                case ScreenSplitAnimationType.LeftHalfToLeft:
                    Camera.depth = 1;
                    Camera.orthographicSize = 18;
                    Side = ScreenSplit.ScreenSide.LEFT;
                    break;
                case ScreenSplitAnimationType.EmptyToRight:
                case ScreenSplitAnimationType.FullToRight:
                case ScreenSplitAnimationType.RightHalfToRight:
                    Camera.depth = 1;
                    Camera.orthographicSize = 18;
                    Side = ScreenSplit.ScreenSide.RIGHT;
                    break;
                case ScreenSplitAnimationType.EmptyToMiddle:
                case ScreenSplitAnimationType.FullToMiddle:
                case ScreenSplitAnimationType.LeftHalfToMiddle:
                case ScreenSplitAnimationType.RightHalfToMiddle:
                    Camera.depth = 1;
                    Camera.orthographicSize = 18;
                    Side = ScreenSplit.ScreenSide.MIDDLE;
                    break;
                
                case ScreenSplitAnimationType.VanishFromFull:
                case ScreenSplitAnimationType.VanishFromLeft:
                case ScreenSplitAnimationType.VanishFromMiddle:
                case ScreenSplitAnimationType.VanishFromRight:
                case ScreenSplitAnimationType.VanishFromLeftHalf:
                case ScreenSplitAnimationType.VanishFromRightHalf:
                Camera.depth = 1;
                break;
                    
            }

            if (friendName != FriendName.None)
            {
                Character.gameObject.SetActive(true);
                Character.transform.position = GameManager.instance.StartPosition.position;
                Character.Friend.Name = friendName;
            }
        }

        private static readonly int VanishHash = Animator.StringToHash("Vanish");

        public void Vanish()
        {
            Character.FirstJumped = false;
            Character.gameObject.SetActive(false);
        }
    }
}