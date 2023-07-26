using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace dkstlzu.Utility
{
    public static class EnumHelper
    {
        public static void ClapIndexOfEnum(Type enumType, int from, int to, out int startIndex, out int endIndex)
        {
            startIndex = -1;
            endIndex = -1;

            int[] intarr = (int[])Enum.GetValues(enumType);
            bool startCheck = false, endCheck = false;

            if (intarr[intarr.Length-1] < from) startCheck = true;
            if (intarr[0] > to) endCheck = true;

            if (startCheck && endCheck) return;

            for (int i = 0; i < intarr.Length; i++)
            {

                if (startCheck && endCheck) return;

                if (intarr[i] >= from && intarr[i] <= to && !startCheck)
                {
                    startIndex = i;
                    startCheck = true;
                }

                if (intarr[i] >= to && !endCheck)
                {
                    endIndex = i-1;
                    endCheck = true;
                }
            }
            if (endIndex < 0) endIndex = intarr.Length-1;
        }
        
        public static string[] ClapNamesOfEnum<EnumType>(int from, int to = Int32.MaxValue) where EnumType : Enum
        {
            List<string> enumList = new List<string>();
            string[] enumNames = Enum.GetNames(typeof(EnumType));
            List<string> result = new List<string>();

            for (int i = from; i < to; i++)
            {
                object value;
                if (Enum.TryParse(typeof(EnumType), enumNames[i], true, out value))
                {
                    if ((int)value >= from && (int)value <= to)
                    {
                        result.Add(enumNames[i]);
                    }
                }
            }
            
            return result.ToArray();
        }

        public static EnumType[] ClapValuesOfEnum<EnumType>(int from, int to = Int32.MaxValue) where EnumType : Enum
        {
            List<EnumType> enumList = new List<EnumType>();
            enumList.AddRange((EnumType[])Enum.GetValues(typeof(EnumType)));
            List<EnumType> result = new List<EnumType>();

            foreach (EnumType value in enumList)
            {
                if ((int)(object)value >= from && (int)(object)value <= to)
                {
                    result.Add(value);
                }
            }
            
            return result.ToArray();
        }
        
        
        public static void ClapIndexOfEnum<EnumType>(int from, int to, out int startIndex, out int endIndex) where EnumType : Enum
        {
            ClapIndexOfEnum(typeof(EnumType), from, to, out startIndex, out endIndex);
        }

        public static Type GetEnumType(string enumName)
        {
            Type type = null;
            try
            {
                type = Type.GetType(enumName + ", Assembly-CSharp-firstpass", true, true);
            } catch(TypeLoadException)
            {
                UnityEngine.Debug.LogWarning("Wrong EnumName of Assembly-CSharp-firstpass in EnumHelper.GetEnumType(). Check Assembly or NameSpace");
                try
                {
                    type = Type.GetType(enumName + ", Assembly-CSharp", true, true);
                } catch(TypeLoadException)
                {
                    UnityEngine.Debug.LogWarning("Wrong EnumName of Assembly-CSharp in EnumHelper.GetEnumType(). Check Assembly or NameSpace");
                }
            }

            return type;
        }

        public static int GetFirstValue(Type type)
        {
            int[] values = (int[])Enum.GetValues(type);
            return values[0];
        }

        public static int GetIndexOf(Enum enumValue)
        {
            return Array.IndexOf(Enum.GetValues(enumValue.GetType()), enumValue);
        }
        public static int GetIndexOf(string enumValueString, Type type)
        {
            return Array.IndexOf(Enum.GetValues(type), Enum.Parse(type, enumValueString) as Enum);
        }
        public static int GetIndexOf<TEnum>(string enumValueString)
        {
            return Array.IndexOf(Enum.GetValues(typeof(TEnum)), Enum.Parse(typeof(TEnum), enumValueString) as Enum);
        }
    }
}
