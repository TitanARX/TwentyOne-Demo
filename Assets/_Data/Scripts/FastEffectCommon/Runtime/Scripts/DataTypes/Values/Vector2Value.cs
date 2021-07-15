using System;
using UnityEngine;

namespace Com.FastEffect.DataTypes
{
    [CreateAssetMenu(menuName = "Fast Effect / Data Types / Vector2 Value")]

    public class Vector2Value : AbstractValue<Vector2>
    {
        [SerializeField]
        public float x { get{return m_value.x;} set{m_value.x = value;}}
        [SerializeField]
        public float y { get{return m_value.y;} set{m_value.y = value;}}

    }

    [Serializable]
    public class Vector2Reference : AbstractReference<Vector2>
    {
        public Vector2Reference()
        {
        }

        public Vector2Reference(Vector2 value) : base(value)
        {
        }
        [SerializeField]
        public float x { get{return Value.x;} 
        set{
            m_constantValue.x = value;

            if (!m_useConstant)
            {
                Vector2 temp = m_variableObject.Value;
                temp.x = value;
                m_variableObject.Value = temp;
            }
            }}
        [SerializeField]
        public float y { get{return Value.y;}
        set{
            m_constantValue.y = value;

            if (!m_useConstant)
            {
                Vector2 temp = m_variableObject.Value;
                temp.y = value;
                m_variableObject.Value = temp;
            }
            }}
    }
}