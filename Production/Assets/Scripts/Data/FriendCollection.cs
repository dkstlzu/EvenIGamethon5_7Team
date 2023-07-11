using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoonBunny
{
    [CreateAssetMenu(menuName = "Collection", fileName = "FriendCollection")]
    public class FriendCollection : ScriptableObject
    {
        [Serializable]
        public class Data : IEquatable<Data>
        {
            public FriendName Name;
            public int TargetCollectingNumber;
            public int CurrentCollectingNumber;

            public bool Finish()
            {
                if (TargetCollectingNumber <= CurrentCollectingNumber) return true;
                else return false;
            }

            public bool Equals(Data other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Name == other.Name;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Data)obj);
            }

            public override int GetHashCode()
            {
                return (int)Name;
            }
        }

        public List<Data> Datas;
    }
}