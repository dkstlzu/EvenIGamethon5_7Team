using System;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace dkstlzu.Utility
{
    public class IntValue : System.Attribute
    {
        private int _value; 

        public IntValue(int value) 
        { 
            _value = value; 
        } 

        public int Value 
        { 
            get {return _value;}
        } 

        public static int GetEnumIntValue(Object value)
        {
            int output = 0;
            
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            IntValue[] attrs = fi.GetCustomAttributes(typeof(IntValue), false) as IntValue[];
            
            if(attrs.Length > 0)
            {
                output = attrs[0].Value;
            }
                
            return output;
        }

        public static T GetEnumValue<T>(int value) where T : Enum
        {
            Type type = typeof(T);

            T[] TArr = Enum.GetValues(typeof(T)) as T[];

            foreach (T t in TArr)
            {
                FieldInfo Tfi = type.GetField(t.ToString());

                IntValue[] attrs = Tfi.GetCustomAttributes<IntValue>(false) as IntValue[];

                if (attrs.Length <= 0) continue;

                foreach (IntValue intValue in attrs)
                {
                    if (intValue.Value == value)
                    {
                        return t;
                    }
                }
            }
            
            return TArr[0];
        }

        
    }
}
