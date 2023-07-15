using System;
using dkstlzu.Utility;
using MoonBunny.UIs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonBunny
{
    public enum StageName
    {
        [StringValue(SceneName.Stage1)]
        GrassField,
        [StringValue(SceneName.Stage2)]
        FairyForest,
        [StringValue(SceneName.Stage3)]
        CottonCandySky,
        [StringValue(SceneName.Stage4)]
        HoneyTastedAurora,
        [StringValue(SceneName.Stage5)]
        StarfulMilkyWay,
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