using UnityEngine;

namespace EvenI7.Proto_Movement
{
    public class CameraFollower : MonoBehaviour
    {
        public Transform Target;
        public Vector2 Offset;
        void Update()
        {
            transform.position = new Vector3(Target.position.x + Offset.x, Target.position.y + Offset.y, -10);
        }
    }
}