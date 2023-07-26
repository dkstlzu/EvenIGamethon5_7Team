﻿using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using MoonBunny.UIs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace MoonBunny
{
    public enum StageName
    {
        [StringValue(SceneName.Stage1_1)]
        [StringValue(SceneName.Stage1_2)]
        [StringValue(SceneName.Stage1_3)]
        [IntValue(1)]
        GrassField,
        [StringValue(SceneName.Stage2_1)]
        [StringValue(SceneName.Stage2_2)]
        [StringValue(SceneName.Stage2_3)]
        [IntValue(2)]
        FairyForest,
        [StringValue(SceneName.Stage3_1)]
        [StringValue(SceneName.Stage3_2)]
        [StringValue(SceneName.Stage3_3)]
        [IntValue(3)]
        CottonCandySky,
        [StringValue(SceneName.Stage4_1)]
        [StringValue(SceneName.Stage4_2)]
        [StringValue(SceneName.Stage4_3)]
        [IntValue(4)]
        HoneyTastedAurora,
        [StringValue(SceneName.Stage5_1)]
        [StringValue(SceneName.Stage5_2)]
        [StringValue(SceneName.Stage5_3)]
        [IntValue(5)]
        StarfulMilkyWay,
        [StringValue(SceneName.StageChallenge)]
        [IntValue(6)]
        Challenge,
    }
    public class Stage : MonoBehaviour
    {
        public StageName Name;

        public int StageLevel
        {
            get => IntValue.GetEnumIntValue(Name);
        }

        public int SubLevel;

        [SerializeField] private Transform _startPoint;

        [SerializeField] private StageSpec _spec;
        public StageSpec Spec => _spec;

        public ReadOnlyEnumDict<FriendName, int> CollectDict;

        private LevelSummoner _summoner;

        public StageUI UI;
        
        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                UI.SetScore(_score);
            }
        }

        private Character _character;
        private float _realHeight =>  GridTransform.GridSetting.GridHeight *  _spec.Height;
        [SerializeField] private SpriteRenderer _backgroundSpriteRenderer;
        [SerializeField] private BoxCollider2D _leftWallCollider;
        [SerializeField] private BoxCollider2D _rightWallCollider;
        [SerializeField] private PolygonCollider2D _levelCollider;
        public PolygonCollider2D LevelConfiner => _levelCollider;
        
        public void Awake()
        {
            foreach (var friendName in EnumHelper.ClapValuesOfEnum<FriendName>(0))
            {
                CollectDict.Add(friendName, 0);    
            }
            
            GameManager.instance.Stage = this;
            UI.Stage = this;
            _summoner = GetComponent<LevelSummoner>();
            Name = StringValue.GetEnumValue<StageName>(SceneManager.GetActiveScene().name);

            _spec = Resources.Load<StageSpec>($"Specs/Stage{StageLevel}Spec");

            Vector3 backgroundPosition = _backgroundSpriteRenderer.transform.position;
            _backgroundSpriteRenderer.transform.position = new Vector3(backgroundPosition.x, _realHeight/2, backgroundPosition.z);
            _backgroundSpriteRenderer.size = new Vector2(_backgroundSpriteRenderer.size.x, _realHeight + 20);

            Vector3 leftWallPosition = _leftWallCollider.transform.position;
            _leftWallCollider.transform.position = new Vector3(leftWallPosition.x, _realHeight/2, leftWallPosition.z);
            _leftWallCollider.size = new Vector2(_leftWallCollider.size.x, _realHeight + 20);
            
            Vector3 rightWallPosition = _rightWallCollider.transform.position;
            _rightWallCollider.transform.position = new Vector3(rightWallPosition.x, _realHeight/2, rightWallPosition.z);
            _rightWallCollider.size = new Vector2(_rightWallCollider.size.x, _realHeight + 20);

            Vector2 minPoint = GridTransform.ToReal(new Vector2Int(GridTransform.GridXMin, -_spec.Height / 2)) -
                               GridTransform.GetGridSize() / 2 + Vector2.down * 3;
            Vector2 maxPoint = GridTransform.ToReal(new Vector2Int(GridTransform.GridXMax, _spec.Height/2)) +
                               GridTransform.GetGridSize() / 2;

            Vector2[] path = new Vector2[]
            {
                new Vector2(minPoint.x, maxPoint.y),
                minPoint,
                new Vector2(maxPoint.x, minPoint.y),
                maxPoint
            };

            _levelCollider.SetPath(0, path);
        }

        private void Start()
        {
            _character = GameObject.FindWithTag("Player").GetComponent<Character>();
            _character.Rigidbody.ForcePosition(_startPoint.position);
            _character.Rigidbody.PauseMove();
            _summoner.SummonRicecakes();
            _summoner.SummonCoins();
            _summoner.SummonFriendCollectables();
        }

        public void CountDownFinish()
        {
            _character.Rigidbody.UnpauseMove();
            _character.StartJump();
            SoundManager.instance.PlayClip(PreloadedResources.instance.OpenStageAudioClip);
        }

        public void Clear()
        {
            foreach (var element in CollectDict)
            {
                FriendCollectionManager.instance.Collect(element.Key, element.Value);
            }

            GameManager.instance.ClearDict[Name] = Mathf.Max(GameManager.instance.ClearDict[Name], SubLevel+1);
            
            GameManager.instance.SaveCollection();
            
            UI.Clear();
            MoonBunnyRigidbody.DisableAll();
        }

        public void Fail()
        {
            UI.Fail();
            MoonBunnyRigidbody.DisableAll();
        }
    }
}