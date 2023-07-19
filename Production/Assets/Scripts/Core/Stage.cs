using System;
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
        GrassField1,
        [StringValue(SceneName.Stage1_2)]
        GrassField2,
        [StringValue(SceneName.Stage1_3)]
        GrassField3,
        [StringValue(SceneName.Stage2_1)]
        FairyForest1,
        [StringValue(SceneName.Stage2_2)]
        FairyForest2,
        [StringValue(SceneName.Stage2_3)]
        FairyForest3,
        [StringValue(SceneName.Stage3_1)]
        CottonCandySky1,
        [StringValue(SceneName.Stage3_2)]
        CottonCandySky2,
        [StringValue(SceneName.Stage3_3)]
        CottonCandySky3,
        [StringValue(SceneName.Stage4_1)]
        HoneyTastedAurora1,
        [StringValue(SceneName.Stage4_2)]
        HoneyTastedAurora2,
        [StringValue(SceneName.Stage4_3)]
        HoneyTastedAurora3,
        [StringValue(SceneName.Stage5_1)]
        StarfulMilkyWay1,
        [StringValue(SceneName.Stage5_2)]
        StarfulMilkyWay2,
        [StringValue(SceneName.Stage5_3)]
        StarfulMilkyWay3,
        [StringValue(SceneName.StageChallenge)]
        Challenge,
    }
    public class Stage : MonoBehaviour
    {
        public StageName Name;

        public int StageLevel
        {
            get => ((int)Name / 3) + 1;
        }

        [SerializeField] private StageSpec _spec;
        public StageSpec Spec => _spec;

        private LevelSummoner _summoner;

        public StageUI UI;

        private Character _character;
        private float _realHeight;
        
        public void Awake()
        {
            GameManager.instance.Stage = this;
            UI.Stage = this;
            _summoner = GetComponent<LevelSummoner>();
            Name = StringValue.GetEnumValue<StageName>(SceneManager.GetActiveScene().name);
        }

        private void Start()
        {
            _character = GameObject.FindWithTag("Player").GetComponent<Character>();
            _summoner.SummonRicecakes();
            _summoner.SummonCoins();
            _realHeight = GridTransform.GridSetting.GridHeight * _spec.Height;
        }

        public void CountDownFinish()
        {
            _character.StartJump();
            SoundManager.instance.PlayClip(PreloadedResources.instance.OpenStageAudioClip);
        }

        public void Clear()
        {
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