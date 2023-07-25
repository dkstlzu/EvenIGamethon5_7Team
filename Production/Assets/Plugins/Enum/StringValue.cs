using System;
using System.Reflection;

namespace dkstlzu.Utility
{
    [AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class StringValue : System.Attribute 
    { 

        private string _value; 

        public StringValue(string value) 
        { 
            _value = value; 
        } 

        public string Value 
        { 
            get {return _value;}
        } 

        public static string GetStringValue(Object value, int index = 0)
        {
            Type type = value.GetType();
            
            FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs = fi.GetCustomAttributes(typeof(StringValue), false) as StringValue[];

            return attrs[index].Value;
        }

        public static T GetEnumValue<T>(string value) where T : Enum
        {
            Type type = typeof(T);

            T[] TArr = Enum.GetValues(typeof(T)) as T[];

            foreach (T t in TArr)
            {
                FieldInfo Tfi = type.GetField(t.ToString());
                
                StringValue[] attrs = Tfi.GetCustomAttributes<StringValue>(false) as StringValue[];

                if (attrs.Length <= 0) continue;

                foreach (StringValue strValue in attrs)
                {
                    if (strValue.Value == value)
                    {
                        return t;
                    }
                }
            }
            
            return TArr[0];
        }
    }
}