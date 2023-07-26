using dkstlzu.Utility;
using UnityEditor;

namespace MoonBunny.Dev.Editor
{
    [CustomPropertyDrawer(typeof(EnumDictElement<FriendName, int>))]
    public class FrienNameIntElementPropertyDrawer : EnumDictElementPropertyDrawer<FriendName, int> {}
    
    [CustomPropertyDrawer(typeof(EnumDictElement<StageName, int>))]
    public class StageNameIntElementPropertyDrawer : EnumDictElementPropertyDrawer<StageName, int> {}
    
    [CustomPropertyDrawer(typeof(ReadOnlyEnumDict<FriendName, int>))]
    public class ROFriendNameIntDictPropertyDrawer : ReadOnlyEnumDictPropertyDrawer<FriendName, int> {}
    
    [CustomPropertyDrawer(typeof(ReadOnlyEnumDict<StageName, int>))]
    public class ROStageNameIntDictPropertyDrawer : ReadOnlyEnumDictPropertyDrawer<StageName, int> {}
    
    [CustomPropertyDrawer(typeof(EnumDict<FriendName, int>))]
    public class RWFriendNameIntDictPropertyDrawer : EnumDictPropertyDrawer<FriendName, int> {}
        
    [CustomPropertyDrawer(typeof(EnumDict<FriendName, float>))]
    public class RWFriendNameFloatDictPropertyDrawer : EnumDictPropertyDrawer<FriendName, float> {}
    
    [CustomPropertyDrawer(typeof(EnumDict<StageName, int>))]
    public class RWStageNameIntDictPropertyDrawer : EnumDictPropertyDrawer<StageName, int> {}
}