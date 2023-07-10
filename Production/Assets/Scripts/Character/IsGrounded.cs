using UnityEngine;

namespace MoonBunny
{
    public enum GroundCheckType
    {
        Line,
        Box,
    }
    
    public class IsGrounded
    {
        public Transform TargetTransform;
        public float CheckDistance = 0.1f;
        public int GroundLayerMask = LayerMask.GetMask("Platform");
        public GroundCheckType Type;
        private bool isGrounded;

        public bool Check()
        {
            if (Type == GroundCheckType.Line)
            {
                RaycastHit2D hit;

                if (hit = Physics2D.Raycast(TargetTransform.transform.position, Vector2.down, CheckDistance, GroundLayerMask))
                {
                    isGrounded = true;
                }
                else
                {
                    isGrounded = false;
                }
            } else if (Type == GroundCheckType.Box)
            {
                RaycastHit2D hit;
                
                if (hit = Physics2D.BoxCast(TargetTransform.transform.position, Vector2.one, 0, Vector2.down, CheckDistance, GroundLayerMask))
                {
                    isGrounded = true;
                }
                else
                {
                    isGrounded = false;
                }
            }
            
            Debug.Log(isGrounded);
            return isGrounded;
        }

        public static implicit operator bool(IsGrounded isGrounded) => isGrounded.Check();
    }
} 