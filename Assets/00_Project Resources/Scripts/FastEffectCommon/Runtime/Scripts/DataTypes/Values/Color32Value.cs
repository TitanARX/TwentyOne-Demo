using System;
using UnityEngine;

namespace Com.FastEffect.DataTypes
{
    [CreateAssetMenu(menuName = "Fast Effect / Data Types / Color32 Value")]

    public class Color32Value : AbstractValue<Color32>
    {
        public static implicit operator Color(Color32Value reference)
        {
            return reference.Value;
        }
    }
    [Serializable]
    public class Color32Reference : AbstractReference<Color32>
    {
        public Color32Reference()
        {
        }

        public Color32Reference(Color32 value) : base(value)
        {
        }
        public static implicit operator Color(Color32Reference reference)
        {
            return reference.Value;
        }
    }
}