using System;
using UnityEngine;
namespace Com.FastEffect.DataTypes
{
    [CreateAssetMenu(menuName = "Fast Effect / Runtime Sets / Sprite Set")]
    public class SpriteSet : RuntimeSet<Sprite>
    {

    }

    [Serializable]
    public class SpriteSetReference : RuntimeSetReference<Sprite>
    {

        public SpriteSetReference()
        {
            m_useConstant = true;
        }
    }
}