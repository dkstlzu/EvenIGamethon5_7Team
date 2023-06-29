using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dkstlzu.Utility
{
    [DefaultExecutionOrder(-100)]
    public class UniqueComponent : MonoBehaviour
    {

        public enum UniqueComponentMethod
        {
            Normal,
            Unique,
            UniqueID,
        }

        public UniqueComponentMethod Uniqueness;
        public Component TargetComponent;
        public string HashID;
        public GameObject DestroyGameObject;
        [Tooltip("Replace old one with new one if duplicate")]
        public bool ReplacePreviousOne = false;

        static Dictionary<object, Component> ComponentsDict = new Dictionary<object, Component>();

        void Awake()
        {
            if (!_Add())
            {
                if (DestroyGameObject) DestroyImmediate(DestroyGameObject);
                else if (TargetComponent is Transform || TargetComponent is RectTransform) DestroyImmediate(TargetComponent.gameObject);
                else DestroyImmediate(TargetComponent);

                Destroy(this);
            }
        }

        public void DestroyTarget()
        {
            StartCoroutine(UniqueOnDestroyInvoker());
        }

        IEnumerator UniqueAwakeInvoker()
        {
            yield return null;
            if (TargetComponent)
                TargetComponent.SendMessage("UniqueAwake", SendMessageOptions.DontRequireReceiver);
        }

        IEnumerator UniqueOnDestroyInvoker()
        {
            if (TargetComponent)
                TargetComponent.SendMessage("UniqueOnDestroy", SendMessageOptions.DontRequireReceiver);
            yield return null;
        }

        private bool _Add()
        {
            return Add(TargetComponent, Uniqueness, HashID, ReplacePreviousOne);
        }

        public static bool Add(Component component, UniqueComponentMethod method, string HashID, bool resplacePrevious = false)
        {
            switch (method)
            {
                case UniqueComponentMethod.Normal:
                return AddNormal(component, resplacePrevious);
                case UniqueComponentMethod.Unique:
                return AddUnique(component, resplacePrevious);
                case UniqueComponentMethod.UniqueID:
                if (HashID == string.Empty) Debug.LogError($"{component.gameObject}'s DDM has wrong Unique ID");
                return AddUniqueID(component, HashID, resplacePrevious);
                default: return false;
            }
        }

        static bool AddNormal(Component component, bool resplacePrevious = false)
        {
            ComponentsDict.Add(component.GetHashCode(), component);
            return true;
        }

        static bool AddUnique(Component component, bool replacePrevious = false)
        {
            bool result = false;

            if (ComponentsDict.ContainsKey(component.GetType()))
            {
                if (replacePrevious)
                {
                    ComponentsDict[component.GetType()].GetComponent<UniqueComponent>().DestroyTarget();
                    ComponentsDict[component.GetType()] = component;
                    result = true;
                } else
                {
                    result = false;
                }
            } else
            {
                ComponentsDict[component.GetType()] = component;
                result = true;
            }

            return result;
        }

        static bool AddUniqueID(Component component, string hashID, bool replacePrevious = false)
        {
            bool result = false;

            if (ComponentsDict.ContainsKey(hashID))
            {
                if (replacePrevious)
                {
                    ComponentsDict[hashID].GetComponent<UniqueComponent>().DestroyTarget();
                    ComponentsDict[hashID] = component;
                    result = true;
                } else
                {
                    result = false;
                }
            } else
            {
                ComponentsDict[hashID] = component;
                result = true;
            }

            return result;
        }
    }
}