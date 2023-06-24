using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = System.Random;

namespace EvenI7.ProtoScreenSplit
{
    public class ScreenSplit : Singleton<ScreenSplit>
    {
        public enum ScreenSide
        {
            NONE = -1,
            LEFT,
            RIGHT,
            MIDDLE,
        }
        
        public int MaxSplitScreenNumber;
        public int CurrentSplitedScreenNumber;
        public bool isSpliting;

        [SerializeField] private SplitedScreen _left;
        [SerializeField] private SplitedScreen _right;
        [SerializeField] private SplitedScreen _middle;

        public List<SplitedScreen> SplitedScreenQueue;

        public SplitedScreen LEFT
        {
            get => _left;
            private set => _left = value;
        }
    
        public SplitedScreen RIGHT
        {
            get => _right;
            private set => _right = value;
        }

        public SplitedScreen MIDDLE
        {
            get => _middle;
            private set => _middle = value;
        }

        private Moon _moon;
        private void Awake()
        {
            _moon = GameObject.FindWithTag("Moon").GetComponent<Moon>();
            // _moon.OnFriendCharacterArrivedToMoon +=
        }

        public void DestroyScreen(ScreenSide side)
        {
            switch (side)
            {
                case ScreenSide.LEFT:
                    break;
                case ScreenSide.RIGHT:
                    break;
                case ScreenSide.MIDDLE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        public SplitedScreen AddNewScreen(FriendName withFriend)
        {
            if (CurrentSplitedScreenNumber == 1)
            {
                if (UnityEngine.Random.value > 0.5)
                {
                    return CreateNewScreen(ScreenSide.RIGHT, withFriend);
                }
                else
                {
                    CreateNewScreen(ScreenSide.LEFT, withFriend);
                }
            } else if (CurrentSplitedScreenNumber == 2)
            {
                return CreateNewScreen(ScreenSide.MIDDLE, withFriend);
            }
            else
            {
                Debug.LogWarning($"Wrong Behaviour of ScreenSplit.AddNewScreen CurrentNumber {CurrentSplitedScreenNumber} Check again");
            }
            
            return null;
        }

        private SplitedScreen CreateNewScreen(ScreenSide side, FriendName withFriend)
        {
            switch (side)
            {
                case ScreenSide.LEFT:
                    if (MIDDLE)
                    {
                        RIGHT = MIDDLE;
                        MIDDLE = null;
                        LEFT = SplitedScreenQueue.First();
                        SplitedScreenQueue.RemoveAt(0);
                        
                        RIGHT.Split(ScreenSplitAnimationType.FullToRightHalf);
                        LEFT.Split(ScreenSplitAnimationType.EmptyToLeftHalf, withFriend);

                        RIGHT.Camera.depth = 1;
                        LEFT.Camera.depth = 1;
                        SplitedScreenQueue[0].Camera.depth = -1;
                    } else if (LEFT && RIGHT)
                    {
                        MIDDLE = LEFT;
                        LEFT = SplitedScreenQueue.First();
                        SplitedScreenQueue.RemoveAt(0);
                        
                        RIGHT.Split(ScreenSplitAnimationType.RightHalfToRight);
                        MIDDLE.Split(ScreenSplitAnimationType.LeftHalfToMiddle);
                        LEFT.Split(ScreenSplitAnimationType.EmptyToLeftHalf, withFriend);

                        RIGHT.Camera.depth = 1;
                        MIDDLE.Camera.depth = 1;
                        LEFT.Camera.depth = 1;
                    }

                    CurrentSplitedScreenNumber++;
                    return LEFT;
                    break;
                case ScreenSide.RIGHT:
                    if (MIDDLE)
                    {
                        LEFT = MIDDLE;
                        MIDDLE = null;
                        RIGHT = SplitedScreenQueue.First();
                        SplitedScreenQueue.RemoveAt(0);
                        
                        LEFT.Split(ScreenSplitAnimationType.FullToLeftHalf);
                        RIGHT.Split(ScreenSplitAnimationType.EmptyToRightHalf, withFriend);

                        LEFT.Camera.depth = 1;
                        RIGHT.Camera.depth = 1;
                        SplitedScreenQueue[0].Camera.depth = -1;
                    } else if (LEFT && RIGHT)
                    {
                        MIDDLE = RIGHT;
                        RIGHT = SplitedScreenQueue.First();
                        SplitedScreenQueue.RemoveAt(0);
                        
                        LEFT.Split(ScreenSplitAnimationType.LeftHalfToLeft);
                        MIDDLE.Split(ScreenSplitAnimationType.RightHalfToMiddle);
                        RIGHT.Split(ScreenSplitAnimationType.EmptyToRightHalf, withFriend);

                        LEFT.Camera.depth = 1;
                        MIDDLE.Camera.depth = 1;
                        RIGHT.Camera.depth = 1;
                    }

                    CurrentSplitedScreenNumber++;
                    return RIGHT;
                    break;
                case ScreenSide.MIDDLE:
                    if (CurrentSplitedScreenNumber != 2)
                    {
                        Debug.LogWarning(
                            $"Wrong Behaviour of ScreenSplit.CreateNewScene CurrentNumber {CurrentSplitedScreenNumber} Check again");
                        return null;
                    }

                    MIDDLE = SplitedScreenQueue.First();
                    SplitedScreenQueue.RemoveAt(0);
                    
                    LEFT.Split(ScreenSplitAnimationType.LeftHalfToLeft);
                    MIDDLE.Split(ScreenSplitAnimationType.EmptyToMiddle, withFriend);
                    RIGHT.Split(ScreenSplitAnimationType.RightHalfToRight);

                    LEFT.Camera.depth = 1;
                    MIDDLE.Camera.depth = 1;
                    RIGHT.Camera.depth = 1;

                    CurrentSplitedScreenNumber++;
                    return MIDDLE;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        public ScreenSide GetSide(Vector2 pointOnScreen)
        {
            if (CurrentSplitedScreenNumber == 1)
            {
                return ScreenSide.MIDDLE;
            } else if (CurrentSplitedScreenNumber == 2)
            {
                if (pointOnScreen.x <= Screen.width / 2)
                {
                    return ScreenSide.LEFT;
                }
                else
                {
                    return ScreenSide.RIGHT;
                }
            } else if (CurrentSplitedScreenNumber == 3)
            {
                if (pointOnScreen.x <= Screen.width / 3)
                {
                    return ScreenSide.LEFT;
                } else if (pointOnScreen.x <= Screen.width * 2 / 3)
                {
                    return ScreenSide.RIGHT;
                }
                else
                {
                    return ScreenSide.MIDDLE;
                }
            }

            return ScreenSide.LEFT;
        }

        public SplitedScreen GetScreen(ScreenSide side)
        {
            switch (side)
            {
                case ScreenSide.LEFT:
                    return LEFT;
                    break;
                case ScreenSide.RIGHT:
                    return RIGHT;
                    break;
                case ScreenSide.MIDDLE:
                    return MIDDLE;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }
        
    }
}