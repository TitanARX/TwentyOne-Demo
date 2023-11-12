using System;
using UnityEngine;

namespace Com.FastEffect.DataTypes
{
    [CreateAssetMenu(menuName = "Fast Effect / Data Types / Int Value")]

    public class IntValue : AbstractValue<int>
    {

    }

    [Serializable]
    public class IntReference : AbstractReference<int>
    {
        public IntReference()
        {
        }

        public IntReference(int value) : base(value)
        {
        }
    }
}