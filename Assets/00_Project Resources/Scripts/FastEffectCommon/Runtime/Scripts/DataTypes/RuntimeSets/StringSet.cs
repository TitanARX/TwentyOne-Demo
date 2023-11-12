using System;
using UnityEngine;

namespace Com.FastEffect.DataTypes
{
    [CreateAssetMenu(menuName = "Fast Effect / Runtime Sets / String Set")]
    public class StringSet : RuntimeSet<string>
    {
        
    }
    [Serializable]
    public class StringSetReference : RuntimeSetReference<string>
    {
        public StringSetReference()
        {
        }
    }
}