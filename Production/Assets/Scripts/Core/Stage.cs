using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using MoonBunny.Effects;
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
        /// <summary>
        /// Stage Level, Sub Level, Gained Star
        /// </summary>
        public static event Action<Stage> OnStageClear;

        /// <summary>
        /// Unlocked StageLevel
        /// </summary>
        public static event Action<int> OnNewStageUnlocked;
        
        /// <summary>
        /// Unlocked SubLevel
        /// </summary>
        public static event Action<int> OnNewLevelUnlocked;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            OnStageClear = null;
            OnNewStageUnlocked = null;
            OnNewLevelUnlocked = null;
        }

        private const string SpecPath = "Specs/Stage/";
        
        public StageName Name;

        public int StageLevel;
        public int SubLevel;

        [SerializeField] private Transform _startPoint;

        [SerializeField] private StageSpec _spec;
        public StageSpec Spec => _spec;

        [SerializeField] private GameObject _tutorialPrefab;

        public ReadOnlyEnumDict<FriendName, int> CollectDict;

        public StageUI UI;
        
        public int GoldNumber;
        public float GoldMultiplier = 1;

        public int GainedStar;
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
        public int RevivedNumber { get; set; }
        private float _realHeight =>  GridTransform.GridSetting.GridHeight *  Spec.Height;
        [SerializeField] private SpriteRenderer _backgroundSpriteRenderer;
        [SerializeField] private BoxCollider2D _leftWallCollider;
        [SerializeField] private BoxCollider2D _rightWallCollider;
        [SerializeField] private PolygonCollider2D _levelCollider;
        public PolygonCollider2D LevelConfiner => _levelCollider;
        
        public LevelSummonerComponent LevelSummoner;

        public List<BoostEffect> BoostEffectList = new List<BoostEffect>();
        
        public void Awake()
        {
            foreach (var friendName in EnumHelper.ClapValuesOfEnum<FriendName>(0))
            {
                CollectDict.Add(friendName, 0);    
            }
            
            GameManager.instance.Stage = this;
            UI.Stage = this;
            
            Name = StringValue.GetEnumValue<StageName>(SceneManager.GetActiveScene().name);

            _spec = Resources.Load<StageSpec>($"{SpecPath}Stage{StageLevel+1}_{SubLevel+1}Spec");
            
            SetCharater();
            SetEnvironments();
            SetSummoner();
        }

        #region Initialize

        void SetCharater()
        {
            _character = GameObject.FindWithTag("Player").GetComponent<Character>();

            _character.Friend.SetBySpec(PreloadedResources.instance.FriendSpecList[(int)GameManager.instance.UsingFriendName]);
            _character.Animator.runtimeAnimatorController = PreloadedResources.instance.CharacterAnimatorControllerList[(int)_character.Friend.Name];
            _character.Rigidbody.ForcePosition(_startPoint.position);
            _character.Rigidbody.PauseMove();
        }

        void SetEnvironments()
        {
            // Background and sidewalls range initialize;
            Vector3 backgroundPosition = _backgroundSpriteRenderer.transform.position;
            _backgroundSpriteRenderer.transform.position = new Vector3(backgroundPosition.x, _realHeight/2, backgroundPosition.z);
            _backgroundSpriteRenderer.size = new Vector2(_backgroundSpriteRenderer.size.x, _realHeight + 20);

            Vector3 leftWallPosition = _leftWallCollider.transform.position;
            _leftWallCollider.transform.position = new Vector3(leftWallPosition.x, _realHeight/2, leftWallPosition.z);
            _leftWallCollider.size = new Vector2(_leftWallCollider.size.x, _realHeight + 20);
            
            Vector3 rightWallPosition = _rightWallCollider.transform.position;
            _rightWallCollider.transform.position = new Vector3(rightWallPosition.x, _realHeight/2, rightWallPosition.z);
            _rightWallCollider.size = new Vector2(_rightWallCollider.size.x, _realHeight + 20);

            Vector2 minPoint = GridTransform.ToReal(new Vector2Int(GridTransform.GridXMin, -Spec.Height / 2)) -
                GridTransform.GetGridSize() / 2 + Vector2.down * 3;
            Vector2 maxPoint = GridTransform.ToReal(new Vector2Int(GridTransform.GridXMax, Spec.Height/2)) +
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

        void SetSummoner()
        {
            // LevelSummoner setting
            LevelSummoner.RicecakeNumber = Spec.RicecakeNumber;
            LevelSummoner.RainbowRicecakeRatio = Spec.RainbowRicecakeRatio;
            LevelSummoner.CoinNumber = Spec.CoinNumber;
            LevelSummoner.FriendCollectableNumber = Spec.FriendCollectableNumber;

            LevelSummoner.SummonThunderEnabled = Spec.SummonThunderEnabled;
            LevelSummoner.SummonThunderInterval = Spec.SummonThunderInterval;
            LevelSummoner.ThunderWarningTime = Spec.ThunderWarningTime;

            LevelSummoner.SummonShootingStarEnabled = Spec.SummonShootingStarEnabled;
            LevelSummoner.SummonShootingStarInterval = Spec.SummonShootingStarInterval;
            LevelSummoner.ShootingStarWarningTime = Spec.ShootingStarWarningTime;

            LevelSummoner.MaxGridHeight = Spec.Height;

            LevelSummoner.SummonRicecakes();
            LevelSummoner.SummonCoins();
            LevelSummoner.SummonFriendCollectables();
            
            UpdateManager.instance.Register(new TimeUpdatable(LevelSummoner, 1));
        }
        

        #endregion

        private void OnDisable()
        {
            UpdateManager.instance?.Unregister(new TimeUpdatable(LevelSummoner, 1));
        }

        public void OnGotoStageSelect()
        {
            
        }

        public void TutorialOn()
        {
            Instantiate(_tutorialPrefab).GetComponent<TutorialUI>();
        }
        
        public void CountDownFinish()
        {
            _character.Rigidbody.UnpauseMove();
            TimeUpdatable.GlobalSpeed = 1;
            _character.StartJump();
            SoundManager.instance.PlayClip(PreloadedResources.instance.OpenStageAudioClip);

            foreach (BoostEffect effect in BoostEffectList)
            {
                effect.Effect();
            }

            LevelSummoner.SummonThunderEnable = LevelSummoner.SummonThunderEnabled;
            LevelSummoner.SummonShootingStarEnable = LevelSummoner.SummonShootingStarEnabled;
        }

        public void Clear()
        {
            foreach (var element in CollectDict)
            {
                FriendCollectionManager.instance.Collect(element.Key, element.Value);
            }

            if (GameManager.ProgressSaveData.ClearDict[Name] <= SubLevel)
            {
                GameManager.ProgressSaveData.ClearDict[Name] = SubLevel+1;
                OnNewLevelUnlocked?.Invoke(SubLevel+1);
                
                if (SubLevel >= 3)
                {
                    OnNewStageUnlocked?.Invoke(StageLevel+1);
                }
            }
            
            GameManager.instance.GoldNumber += (int)(GoldNumber * GoldMultiplier);
            
            GameManager.instance.SaveProgress();
            
            UI.Clear();
            MoonBunnyRigidbody.DisableAll();
            TimeUpdatable.GlobalSpeed = 0;
            OnStageClear?.Invoke(this);
        }

        public void Fail()
        {
            UI.Fail();
            MoonBunnyRigidbody.DisableAll();
            TimeUpdatable.GlobalSpeed = 0;
        }

        public void Revive()
        {
            RevivedNumber++;
            MoonBunnyRigidbody.EnableAll();
            TimeUpdatable.GlobalSpeed = 1;
            new HeartEffect(_character).Effect();
            new InvincibleEffect(_character.Rigidbody, LayerMask.GetMask("Obstacle"), _character.Renderer, 3, _character.InvincibleEffectCurve).Effect();
            _character.FirstJumped = false;
            _character.StartJump(10);
        }
    }
}