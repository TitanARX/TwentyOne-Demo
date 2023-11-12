using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.FastEffect.DataTypes
{
    [CreateAssetMenu(menuName = "Fast Effect / Runtime Sets / Byte Set")]
    public class ByteSet : RuntimeSet<byte>
    {
    }
    [Serializable]
    public class ByteSetReference : RuntimeSetReference<byte>
    {
        public ByteSetReference()
        {
            m_useConstant = true;
        }
        public ByteSetReference(IEnumerable<byte> initialConstList)
        {
            m_useConstant = true;
            m_constantList = new List<byte>(initialConstList);
        }
    }
}