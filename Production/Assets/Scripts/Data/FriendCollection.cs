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
        public class Data : IEquatable<Data>, ICloneable
        {
            public FriendName Name;
            public int TargetCollectingNumber;
            public int CurrentCollectingNumber;

            public bool IsFinish()
            {
                if (TargetCollectingNumber <= CurrentCollectingNumber) return true;
                else return false;
            }

            public float GetPercent()
            {
                if (TargetCollectingNumber == 0) return 1;
                
                return (float)CurrentCollectingNumber / TargetCollectingNumber;
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

            /// <summary><para>Returns a string that represents the current object.</para></summary>
            public override string ToString()
            {
                return $"FriendCollection Data {Name} {CurrentCollectingNumber}/{TargetCollectingNumber}";
            }

            /// <summary><para>Creates a new object that is a copy of the current instance.</para></summary>
            public object Clone()
            {
                Data instance = new Data();
                instance.TargetCollectingNumber = TargetCollectingNumber;
                instance.CurrentCollectingNumber = CurrentCollectingNumber;
                instance.Name = Name;
                return instance;
            }
        }

        public List<Data> Datas = new List<Data>();

        /// <summary><para>Creates a new object that is a copy of the current instance.</para></summary>
        public void CopyTo(FriendCollection target)
        {
            target.Datas.Clear();

            foreach (Data data in Datas)
            {
                target.Datas.Add((Data)data.Clone());
            }
        }
    }
}