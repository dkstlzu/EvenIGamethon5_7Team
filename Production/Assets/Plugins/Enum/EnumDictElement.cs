using System;
using System.Collections.Generic;
using UnityEngine;

namespace dkstlzu.Utility
{
    [Serializable]
    public struct EnumDictElement<T, M> where T : Enum
    {
        public T EnumKey;
        public M Value;
        
        public EnumDictElement(T key, M value)
        {
            EnumKey = key;
            Value = value;
        }
    }

    [Serializable]
    public class ReadOnlyEnumDict<T, M> : Dictionary<T, M>, ISerializationCallbackReceiver where T : Enum
    {
        [SerializeField] private List<EnumDictElement<T, M>> _elementList;

        public void OnAfterDeserialize()
        {
            if(_elementList != null && _elementList.Count > 0)
            {
                this.Clear();
                int n = _elementList.Count;
                for(int i = 0; i < n; ++i)
                {
                    this[_elementList[i].EnumKey] = _elementList[i].Value;
                }

                _elementList = null;
            }
        }

        public void OnBeforeSerialize()
        {
            _elementList = new List<EnumDictElement<T, M>>();

            foreach(var kvp in this)
            {
                _elementList.Add(new EnumDictElement<T, M>(kvp.Key, kvp.Value));
            }
        }
    }
    
    [Serializable]
    public class EnumDict<T, M> : Dictionary<T, M>, ISerializationCallbackReceiver where T : Enum
    {
        [SerializeField] private List<EnumDictElement<T, M>> _elementList;

        public void OnAfterDeserialize()
        {
            if(_elementList != null && _elementList.Count > 0)
            {
                this.Clear();
                int n = _elementList.Count;
                for(int i = 0; i < n; ++i)
                {
                    this[_elementList[i].EnumKey] = _elementList[i].Value;
                }

                _elementList = null;
            }
        }

        public void OnBeforeSerialize()
        {
            _elementList = new List<EnumDictElement<T, M>>();

            foreach(var kvp in this)
            {
                _elementList.Add(new EnumDictElement<T, M>(kvp.Key, kvp.Value));
            }
        }
    }
}