using System;
using UnityEngine;

namespace MoonBunny
{
    public class StageGoal : MonoBehaviour
    {
        public Stage Stage;
        public static event Action<Friend, StageName> OnFriendStageClear;

        private void OnGoal(GameObject go)
        {
            OnFriendStageClear?.Invoke(go.GetComponent<Character>().Friend, Stage.Name);
        }
    }
}