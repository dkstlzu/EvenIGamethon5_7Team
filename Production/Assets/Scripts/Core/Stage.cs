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

        public GameOverUI GameOverUI;

        public void Clear()
        {
            GameOverUI.ClearUI();
        }

        public void Fail()
        {
            GameOverUI.FailUI();
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