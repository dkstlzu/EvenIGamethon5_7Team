using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EvenI7
{
    public class SpawnArea : MonoBehaviour
    {
        public Rect Area;

        public Vector2 GetRandomPoint()
        {
            Vector2 randomPoint = new Vector2();
            randomPoint = new Vector2(transform.position.x + Random.Range( - Area.width / 2, Area.width / 2),
                transform.position.y + Random.Range(- Area.height / 2,  + Area.height / 2));
            return randomPoint;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(transform.position, Area.size);
        }
    }
}