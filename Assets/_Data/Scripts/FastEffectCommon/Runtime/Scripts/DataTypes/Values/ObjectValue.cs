using System;
using UnityEngine;

namespace Com.FastEffect.DataTypes
{
    [CreateAssetMenu(menuName = "Fast Effect / Data Types / Object Value")]
    public class ObjectValue : AbstractValue<object>
    {

    }
    [Serializable]
    public class ObjectReference : AbstractReference<object>
    {
        public ObjectReference()
        {
        }

        public ObjectReference(object value) : base(value)
        {
        }
    }
}