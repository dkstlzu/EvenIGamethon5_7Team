using UnityEngine;

namespace EvenI7
{
    public class LevelSummoner : MonoBehaviour
    {
        [SerializeField] private FlowingObjects _flowingObjects;

        public GameObject TargetObject;
        
        public void Summon()
        {
            _flowingObjects.Summon(TargetObject);
        }
    }
}