using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using dkstlzu.Utility;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Serialization;


namespace MoonBunny
{
    public class ScreenSplit : Singleton<ScreenSplit>
    {
        public enum ScreenSide
        {
            NONE = -1,
            WAITING,
            LEFT,
            RIGHT,
            FULL,
        }

        public enum ScreenSplitDirection
        {
            HORIZONTAL,
            VERTICAL,
        }

        public ScreenSplitDirection SplitDirection;
        public int CurrentSplitedScreenNumber => _watingOne.isValid ? 1 : 2;

        [SerializeField] private SplitedScreen _left;
        [SerializeField] private SplitedScreen _right;
        [SerializeField] private SplitedScreen _full;
        [SerializeField] private SplitedScreen _watingOne;

        public SplitedScreen LEFT => _left.isValid ? _left : null;
        public SplitedScreen RIGHT => _right.isValid ? _right : null;
        public SplitedScreen FULL => _full.isValid ? _full : null;
        public SplitedScreen WAITING => _watingOne.isValid ? _watingOne : null;

        public bool isSplitable => _watingOne.isValid;

        [Range(0, 1)] public float ScreenSpacing;
        [Range(0, 10)] public float ScreenSplitDuration;
        public AnimationCurve ScreenSplitCurve;

        private void Awake()
        {
            SplitedScreen.SScreenSplit = this;
        }

        public void DestroyScreen(ScreenSide side, bool destroyCharacter = true)
        {
            switch (side)
            {
                case ScreenSide.LEFT:
                    break;
                case ScreenSide.RIGHT:
                    break;
            }
        }

        [ContextMenu("AddNewScreenTest")]
        public SplitedScreen AddNewScreenTest()
        {
            return AddNewScreen(FriendName.Second);
        }
        
        public SplitedScreen AddNewScreen(FriendName withFriend)
        {
            if (!isSplitable)
            {
                Debug.LogWarning($"Wrong Behaviour of ScreenSplit.AddNewScreen CurrentNumber {CurrentSplitedScreenNumber} Check again");
                return null;
            }
            
            if (UnityEngine.Random.value > 0.5)
            {
                return CreateNewScreen(ScreenSide.RIGHT, withFriend);
            }
            else
            {
                return CreateNewScreen(ScreenSide.LEFT, withFriend);
            }
        }

        private SplitedScreen CreateNewScreen(ScreenSide side, FriendName withFriend = FriendName.None)
        {
            if (!isSplitable) return null;
            
            ScreenSide otherSide = side == ScreenSide.LEFT ? ScreenSide.RIGHT : ScreenSide.LEFT;

            FULL.MoveDataTo(GetScreen(otherSide, true));
            GetScreen(otherSide, true).isValid = true;
            FULL.isValid = false;

            WAITING.MoveDataTo(GetScreen(side, true));
            GetScreen(side, true).isValid = true;
            WAITING.isValid = false;

            if (withFriend != FriendName.None)
            {
                CreateCharacterInScreen(GetScreen(side), withFriend);
            }
            
            return GetScreen(side);
        }

        public void DestoryCharacterInScreen(SplitedScreen screen)
        {
            // screen.Character.Disable();
        }

        public void CreateCharacterInScreen(SplitedScreen screen, FriendName name)
        {
            // screen.Character.Enable();
            // screen.Character.Friend.Name = name;
        }

        public ScreenSide GetSide(Vector2 pointOnScreen)
        {
            if (isSplitable) return ScreenSide.FULL;
            
            if (pointOnScreen.x <= Screen.width / 2)
            {
                return ScreenSide.LEFT;
            } else
            {
                return ScreenSide.RIGHT;
            }

        }

        public ScreenSide GetSide(Friend withFriend)
        {
            if (LEFT?.Character.Friend == withFriend)
            {
                return ScreenSide.LEFT;
            } else if (RIGHT?.Character.Friend == withFriend)
            {
                return ScreenSide.RIGHT;
            } 
                
            return ScreenSide.NONE;
        }

        public SplitedScreen GetScreen(ScreenSide side, bool containInvalid = false)
        {
            switch (side)
            {
                case ScreenSide.LEFT:
                    if (containInvalid) return _left;
                    else return LEFT;
                case ScreenSide.RIGHT:
                    if (containInvalid) return _right;
                    else return RIGHT;
                case ScreenSide.FULL:
                    if (containInvalid) return _full;
                    else return FULL;
                case ScreenSide.WAITING:
                    if (containInvalid) return _watingOne;
                    else return WAITING;
            }

            return null;
        }

        public SplitedScreen GetScreen(Friend withFriend)
        {
            return GetScreen(GetSide(withFriend));
        }
    }

    [Serializable]
    public class SplitedScreen
    {
        public Character Character;
        public Camera Camera;
        public ScreenSplit.ScreenSide Side;

        public bool isValid = false;
        public static ScreenSplit SScreenSplit;

        private static readonly Rect EMPTY_VIEWPORT = new Rect(0, 0, 0, 1);
        private static readonly Rect FULL_VIEWPORT = new Rect(0, 0, 1, 1);
        private static readonly Rect LEFT_VIEWPORT = new Rect(0, 0, 0.5f, 1);
        private static readonly Rect RIGHT_VIEWPORT = new Rect(0.5f, 0, 0.5f, 1);

        private Rect _currentViewport;
        private Rect _targetViewport;

        private float _timer;

        public IEnumerator UpdateScreen()
        {
            _timer = 0;

            _timer += Time.deltaTime;

            while (_timer <= SScreenSplit.ScreenSplitDuration)
            {
                yield return new WaitForEndOfFrame();

                float lerp = SScreenSplit.ScreenSplitCurve.Evaluate(_timer / SScreenSplit.ScreenSplitDuration);

                float lerpedX = Mathf.Lerp(_currentViewport.x, _targetViewport.x, lerp);
                float lerpedY = Mathf.Lerp(_currentViewport.y, _targetViewport.y, lerp);
                float lerpedWidth = Mathf.Lerp(_currentViewport.width, _targetViewport.width, lerp);
                float lerpedHeight = Mathf.Lerp(_currentViewport.height, _targetViewport.height, lerp);

                Camera.rect = new Rect(lerpedX, lerpedY, lerpedWidth, lerpedHeight);
            }

            _currentViewport = Camera.rect;
        }

        public void Split(FriendName friendName = FriendName.None)
        {
            Split(Side, friendName);
        }
        
        public void Split(ScreenSplit.ScreenSide toSide, FriendName friendName = FriendName.None)
        {
            Debug.Log($"{Camera.name} split from {Side} to {toSide}");

            _currentViewport = Camera.rect;
                
            switch (toSide)
            {
                case ScreenSplit.ScreenSide.WAITING:
                    _targetViewport = EMPTY_VIEWPORT;
                    break;
                case ScreenSplit.ScreenSide.LEFT:
                    _targetViewport = LEFT_VIEWPORT;
                    break;
                case ScreenSplit.ScreenSide.RIGHT:
                    _targetViewport = RIGHT_VIEWPORT;
                    break;
                case ScreenSplit.ScreenSide.FULL:
                    _targetViewport = FULL_VIEWPORT;
                    break;
            }

            Side = toSide;

            SScreenSplit.StartCoroutine(UpdateScreen());
        }

        public void MoveDataTo(SplitedScreen other)
        {
            other.Character = Character;
            other.Camera = Camera;

            Character = null;
            Camera = null;
        }
    } 
}