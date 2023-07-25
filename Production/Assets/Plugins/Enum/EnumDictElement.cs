using System;

namespace dkstlzu.Utility
{
    [Serializable]
    public struct EnumDictElement<T, M>
    {
        public T EnumKey;
        public M Value;
        
        public EnumDictElement(T key, M value)
        {
            EnumKey = key;
            Value = value;
        }
    }
}