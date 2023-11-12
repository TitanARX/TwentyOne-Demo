using System;
using UnityEngine;

namespace Com.FastEffect.DataTypes
{
    [CreateAssetMenu(menuName = "Fast Effect / Data Types / Float Value")]

    public class FloatValue : AbstractValue<float>
    {
        
    }
    [Serializable]
    public class FloatReference : AbstractReference<float>
    {
        public FloatReference()
        {
        }

        public FloatReference(float value) : base(value)
        {
        }
    }
}