using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using dkstlzu.Utility;

namespace MoonBunny.Dev.Editor
{
    [CustomPropertyDrawer(typeof(InheritanceList<Quest>))]
    public class QuestInheritanceListPropertyDrawer : InheritanceListEditor<Quest> {}

    // [CustomPropertyDrawer(typeof(InheritSwapList<Quest>))]
    public class QuestInehritanceSwapListPropertyDrawer : InheritSwapListPropertyDrawer<Quest> {}
}
