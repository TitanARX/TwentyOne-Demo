using System;
using UnityEngine;

namespace Com.FastEffect.DataTypes
{
    [CreateAssetMenu(menuName = "Fast Effect / Data Types / Long Value")]

    public class LongValue : AbstractValue<long>
    {

    }

    [Serializable]
    public class LongReference : AbstractReference<long>
    {
        public LongReference()
        {
        }

        public LongReference(long value) : base(value)
        {
        }
    }
}