using System;
using UnityEngine;

namespace Com.FastEffect.DataTypes
{
    [CreateAssetMenu(menuName = "Fast Effect / Data Types / Vector3 Value")]

    public class Vector3Value : AbstractValue<Vector3>
    {
        [SerializeField]
        public float x { get{return m_value.x;} set{m_value.x = value;}}
        [SerializeField]
        public float y { get{return m_value.y;} set{m_value.y = value;}}
        [SerializeField]
        public float z { get{return m_value.z;} set{m_value.z = value;}}

    }

    [Serializable]
    public class Vector3Reference : AbstractReference<Vector3>
    {
        public Vector3Reference()
        {
        }

        public Vector3Reference(Vector3 value) : base(value)
        {

        }

        [SerializeField]
        public float x { get{return Value.x;} 
        set{
            m_constantValue.x = value;

            if (!m_useConstant)
            {
                Vector3 temp = m_variableObject.Value;
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
                Vector3 temp = m_variableObject.Value;
                temp.y = value;
                m_variableObject.Value = temp;
            }
            }}
        [SerializeField]
        public float z { get{return Value.z;}
        set{
            m_constantValue.z = value;

            if (!m_useConstant)
            {
                Vector3 temp = m_variableObject.Value;
                temp.z = value;
                m_variableObject.Value = temp;
            }
            }}
    }
}