using System;
using UnityEngine;

namespace Com.FastEffect.DataTypes
{
    [CreateAssetMenu(menuName = "Fast Effect / Data Types / String Value")]

    public class StringValue : AbstractValue<string>
    {

    }

    [Serializable]
    public class StringReference : AbstractReference<string>
    {
        public StringReference()
        {
        }

        public StringReference(string value)
        {
            m_useConstant = true;
            m_constantValue = value;
        }
        
    }
}