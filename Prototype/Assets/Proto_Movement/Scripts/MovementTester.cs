using System;
using UnityEngine;

namespace EvenI7.Proto_Movement
{
    public class MovementTester : MonoBehaviour
    {
        public MovementTest MT;
        
        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 200, 100), "Switch move style"))
            {
                if (MT.MoveType == MoveType.Linear) MT.MoveType = MoveType.Additive;
                else if (MT.MoveType == MoveType.Additive)
                {
                    MT.Rigidbody.velocity = Vector2.zero;
                    MT.MoveType = MoveType.Linear;
                }
            } else if (GUI.Button(new Rect(250, 10, 200, 100), "Automove With Acceleration"))
            {
                MT.Rigidbody.velocity = Vector2.zero;
                MT.StartCoroutine(MT.AutoMoveCoroutine());
            }
        }
    }
}