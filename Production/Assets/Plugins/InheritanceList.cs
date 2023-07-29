using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dkstlzu.Utility
{
    [Serializable]
    public class InheritanceList<T> : List<T>
    {
    }

    [Serializable]
    public class InheritSwapList<T> : List<T>
    {
        [HideInInspector] public InheritanceList<T> InheritanceList;
    }
}
