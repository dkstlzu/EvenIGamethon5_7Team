using System;
using System.Collections.Generic;
using UnityEngine;

namespace dkstlzu.Utility
{
    [Serializable]
    public class DictElement<T, M>
    {
        public T Key;
        public M Value;
        
        public DictElement(T key, M value)
        {
            Key = key;
            Value = value;
        }
    }
    
    [Serializable]
    public class DictElementWithClass<T, M> where M : class
    {
        public T Key;
        public M Value;
        
        public DictElementWithClass(T key, M value)
        {
            Key = key;
            Value = value;
        }
    }
    
    [Serializable]
    public class ReadOnlyDict<T, M> : Dictionary<T, M>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<DictElement<T, M>> _elementList;

        public void OnAfterDeserialize()
        {
            if(_elementList != null && _elementList.Count > 0)
            {
                this.Clear();
                int n = _elementList.Count;
                for(int i = 0; i < n; ++i)
                {
                    this[_elementList[i].Key] = _elementList[i].Value;
                }

                _elementList = null;
            }
        }

        public void OnBeforeSerialize()
        {
            _elementList = new List<DictElement<T, M>>();

            foreach(var kvp in this)
            {
                _elementList.Add(new DictElement<T, M>(kvp.Key, kvp.Value));
            }
        }
    }
    
        
    [Serializable]
    public class ReadOnlyWithClassDict<T, M> : Dictionary<T, M>, ISerializationCallbackReceiver where M : class
    {
        [SerializeField] private List<DictElementWithClass<T, M>> _elementList;

        public void OnAfterDeserialize()
        {
            if(_elementList != null && _elementList.Count > 0)
            {
                this.Clear();
                int n = _elementList.Count;
                for(int i = 0; i < n; ++i)
                {
                    this[_elementList[i].Key] = _elementList[i].Value;
                }

                _elementList = null;
            }
        }

        public void OnBeforeSerialize()
        {
            _elementList = new List<DictElementWithClass<T, M>>();

            foreach(var kvp in this)
            {
                _elementList.Add(new DictElementWithClass<T, M>(kvp.Key, kvp.Value));
            }
        }
    }
    
    [Serializable]
    public class Dict<T, M> : Dictionary<T, M>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<DictElement<T, M>> _elementList;

        public void OnAfterDeserialize()
        {
            if(_elementList != null && _elementList.Count > 0)
            {
                this.Clear();
                int n = _elementList.Count;
                for(int i = 0; i < n; ++i)
                {
                    this[_elementList[i].Key] = _elementList[i].Value;
                }

                _elementList = null;
            }
        }

        public void OnBeforeSerialize()
        {
            _elementList = new List<DictElement<T, M>>();

            foreach(var kvp in this)
            {
                _elementList.Add(new DictElement<T, M>(kvp.Key, kvp.Value));
            }
        }
    }
}

    
    
