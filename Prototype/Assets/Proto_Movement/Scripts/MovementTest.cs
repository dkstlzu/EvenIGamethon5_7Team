using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EvenI7.Proto_Movement
{
    public enum MoveType
    {
        Linear,
        Additive,
    }
    public class MovementTest : MonoBehaviour
    {
        public MoveType MoveType;
        public Rigidbody2D Rigidbody;
        public float MoveSpeed;

        public Proto_Movement_InputAsset InputAsset;
        public InputAction AutoMoveInputAction;

        public Transform AutoMoveStartPosition;
        public Transform AutoMoveEndPosition;
        public float AutoMoveTime;
        
        private void Awake()
        {
            InputAsset = new Proto_Movement_InputAsset();
            InputAsset.Enable();

            InputAsset.Game.Move.performed += Move;
            InputAsset.Game.Move.canceled += Move;

            AutoMoveInputAction.Enable();
            AutoMoveInputAction.performed += (context) => StartCoroutine(AutoMoveCoroutine());
        }

        public Vector2 MoveDirectrion;
        private void Move(InputAction.CallbackContext context)
        {
            MoveDirectrion = Vector2.right * context.ReadValue<float>();
        }

        public void OnMoveLeftButtonClicked()
        {
            MoveDirectrion = -Vector2.right;
        }

        public void OnMoveRightButtonClicked()
        {
            MoveDirectrion = Vector2.right;
        }

        public void OnStopButtonClicked()
        {
            MoveDirectrion = Vector2.zero;
            Rigidbody.velocity = Vector2.zero;
        }
        

        private void FixedUpdate()
        {
            if (MoveType == MoveType.Linear)
            {
                transform.Translate(MoveDirectrion * (MoveSpeed * Time.deltaTime));
            } else if (MoveType == MoveType.Additive)
            {
                Rigidbody.AddForce(MoveDirectrion * MoveSpeed, ForceMode2D.Force);
            }
        }

        public IEnumerator AutoMoveCoroutine()
        {
            transform.position = AutoMoveStartPosition.position;
            float distance = AutoMoveEndPosition.position.x - AutoMoveStartPosition.position.x;
            Vector2 moveDireciton = AutoMoveEndPosition.position - AutoMoveStartPosition.position;
            moveDireciton.Normalize();

            float acceleration = ((distance * 2) / AutoMoveTime) / (AutoMoveTime / 2);

            float time = 0;
            while (time < AutoMoveTime / 2)
            {
                Rigidbody.AddForce(moveDireciton * acceleration);
                time += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            while (time < AutoMoveTime)
            {
                Rigidbody.AddForce(-moveDireciton * acceleration);
                time += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            Rigidbody.velocity = Vector2.zero;
        }
    }
}
