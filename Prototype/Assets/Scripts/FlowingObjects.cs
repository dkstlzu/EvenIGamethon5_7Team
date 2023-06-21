using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace EvenI7
{
    public class FlowingObjects : MonoBehaviour
    {
        public List<Transform> FlowingObjs;
        public float FlowingSpeed;
        public Vector2 FlowingDirection;
        public bool AutoDestroyOnExit;

        [SerializeField] private Transform _summonPoint;

        public void Summon(GameObject obj)
        {
            if (!Application.isPlaying) return;
            
            GameObject summonedObj = Instantiate(obj, _summonPoint.position, Quaternion.identity);
            AddObject(summonedObj);
        }

        public void AddObject(GameObject go)
        {
            AddObject(go.transform);
        }

        public void AddObject(Transform transform)
        {
            FlowingObjs.Add(transform);
        }

        public void RemoveObject(GameObject go)
        {
            RemoveObject(go.transform);
        }

        public void RemoveObject(Transform transform)
        {
            FlowingObjs.Remove(transform);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            print($"TriggerExit Check {other.gameObject.name}");
            if (FlowingObjs.Contains(other.transform))
            {
                RemoveObject(other.transform);
            }
            
            if (AutoDestroyOnExit)
                Destroy(other.gameObject);
        }

        private void Update()
        {
            foreach (Transform obj in FlowingObjs)
            {
                if (!obj) continue;
                obj.Translate(FlowingDirection * (Time.deltaTime * FlowingSpeed));
            }
        }
    }
}