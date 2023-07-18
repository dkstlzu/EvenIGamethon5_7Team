using System;
using dkstlzu.Utility;
using MoonBunny.UIs;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public StageSpec Spec;

        private LevelSummoner _summoner;

        public StageUI UI;
        
        public void Awake()
        {
            _summoner = GetComponent<LevelSummoner>();
        }

        private void Start()
        {
            _summoner.SummonRicecakes();
        }

        public void InitStage(StageSpec spec = null)
        {
            if (spec != null)
            {
                Spec = spec;
            }
            
            
        }

        public void Clear()
        {
            UI.Clear();
        }

        public void Fail()
        {
            UI.Fail();
        }
        
        public void Quit()
        {
            Finish();
        }

        public void Finish()
        {
            SceneManager.LoadScene(SceneName.Start);
        }
    }
}