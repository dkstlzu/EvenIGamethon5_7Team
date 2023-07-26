using System;
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
            _backgroundSpriteRenderer.transform.position = new Vector3(backgroundPosition.x, _realHeight, backgroundPosition.z);
            _backgroundSpriteRenderer.size = new Vector2(_backgroundSpriteRenderer.size.x, _realHeight * 2 + 20);

            Vector3 leftWallPosition = _leftWallCollider.transform.position;
            _leftWallCollider.transform.position = new Vector3(leftWallPosition.x, _realHeight, leftWallPosition.z);
            _leftWallCollider.size = new Vector2(_leftWallCollider.size.x, _realHeight * 2 + 20);
            
            Vector3 rightWallPosition = _rightWallCollider.transform.position;
            _rightWallCollider.transform.position = new Vector3(rightWallPosition.x, _realHeight, rightWallPosition.z);
            _rightWallCollider.size = new Vector2(_rightWallCollider.size.x, _realHeight * 2 + 20);
        }

        private void Start()
        {
            _character = GameObject.FindWithTag("Player").GetComponent<Character>();
            _summoner.SummonRicecakes();
            _summoner.SummonCoins();
            _summoner.SummonFriendCollectables();
        }

        public void CountDownFinish()
        {
            _character.StartJump();
            SoundManager.instance.PlayClip(PreloadedResources.instance.OpenStageAudioClip);
        }

        public void Clear()
        {
            foreach (var element in CollectDict)
            {
                GameManager.instance.CollectDict[element.Key] += element.Value;
            }
            
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