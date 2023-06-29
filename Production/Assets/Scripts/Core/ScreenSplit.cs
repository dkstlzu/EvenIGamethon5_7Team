using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoonBunny
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

        public enum ScreenSplitDirection
        {
            HORIZONTAL,
            VERTICAL,
        }

        public ScreenSplitDirection SplitDirection;
        public int CurrentSplitedScreenNumber;

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

        private void Start()
        {
            StageGoal.OnFriendStageClear += OnFriendGoal;
        }

        private void OnDestroy()
        {
            StageGoal.OnFriendStageClear -= OnFriendGoal;
        }

        private void OnFriendGoal(Friend character, StageName stage)
        {
            ScreenSide side = GetSide(character);
            DestroyScreen(side);
        }

        public void DestroyScreen(ScreenSide side, bool destroyCharacter = true)
        {
            SplitedScreenQueue.Add(GetScreen(side));
            
            switch (side)
            {
                case ScreenSide.LEFT:
                    if (MIDDLE)
                    {
                        LEFT.Vanish();
                        MIDDLE.Split(ScreenSplitAnimationType.MiddleToLeftHalf);
                        RIGHT.Split(ScreenSplitAnimationType.RightToRightHalf);

                        LEFT = MIDDLE;
                        MIDDLE = null;
                    }
                    else
                    {
                        LEFT.Vanish();
                        RIGHT.Split(ScreenSplitAnimationType.RightHalfToFull);

                        LEFT = null;
                        MIDDLE = RIGHT;
                        RIGHT = null;
                    }
                    break;
                case ScreenSide.RIGHT:
                    if (MIDDLE)
                    {
                        RIGHT.Vanish();
                        MIDDLE.Split(ScreenSplitAnimationType.MiddleToRightHalf);
                        LEFT.Split(ScreenSplitAnimationType.LeftToLeftHalf);

                        RIGHT = MIDDLE;
                        MIDDLE = null;
                    }
                    else
                    {
                        RIGHT.Vanish();
                        LEFT.Split(ScreenSplitAnimationType.LeftHalfToFull);

                        RIGHT = null;
                        MIDDLE = LEFT;
                        LEFT = null;
                    }
                    break;
                case ScreenSide.MIDDLE:
                    if (CurrentSplitedScreenNumber < 2) return;
                    
                    LEFT.Split(ScreenSplitAnimationType.LeftToLeftHalf);
                    RIGHT.Split(ScreenSplitAnimationType.RightToRightHalf);
                    MIDDLE.Vanish();

                    MIDDLE = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }

            CurrentSplitedScreenNumber--;
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        public void DestoryCharacter(ScreenSide side)
        {
            
        }

        public void CreateCharacter(ScreenSide side)
        {
            
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

        public ScreenSide GetSide(Friend withFriend)
        {
            if (LEFT && LEFT?.Character.Friend == withFriend)
            {
                return ScreenSide.LEFT;
            } else if (MIDDLE && MIDDLE?.Character.Friend == withFriend)
            {
                return ScreenSide.MIDDLE;
            } else if (RIGHT && RIGHT?.Character.Friend == withFriend)
            {
                return ScreenSide.RIGHT;
            } else
            {
                return ScreenSide.NONE;
            }
        }

        public SplitedScreen GetScreen(ScreenSide side)
        {
            switch (side)
            {
                case ScreenSide.LEFT:
                    return LEFT;
                case ScreenSide.RIGHT:
                    return RIGHT;
                case ScreenSide.MIDDLE:
                    return MIDDLE;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        public SplitedScreen GetScreen(Friend withFriend)
        {
            if (LEFT && LEFT?.Character.Friend == withFriend)
            {
                return LEFT;
            } else if (MIDDLE && MIDDLE?.Character.Friend == withFriend)
            {
                return MIDDLE;
            } else if (RIGHT && RIGHT?.Character.Friend == withFriend)
            {
                return RIGHT;
            } else
            {
                return null;
            }
        }
        
    }
}