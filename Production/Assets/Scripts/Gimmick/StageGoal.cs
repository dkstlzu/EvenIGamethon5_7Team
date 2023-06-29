using System;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public class StageGoal : MonoBehaviour
    {
        public Stage Stage;
        private EventTrigger _et;

        public static event Action<Friend, StageName> OnFriendStageClear;
        private void Awake()
        {
            _et = GetComponent<EventTrigger>();
            _et.AddEnterGOEvent(OnGoal);
        }

        private void OnGoal(GameObject go)
        {
            OnFriendStageClear?.Invoke(go.GetComponent<Character>().Friend, Stage.Name);
        }
    }
}