using System;
using UnityEngine;

namespace Com.FastEffect.DataTypes
{
    [CreateAssetMenu(menuName = "Fast Effect / Data Types / Bool Value")]

    public class BoolValue : AbstractValue<bool>
    {

    }

    [Serializable]
    public class BoolReference : AbstractReference<bool>
    {
        public BoolReference()
        {
        }

        public BoolReference(bool value) : base(value)
        {
        }
    }
}