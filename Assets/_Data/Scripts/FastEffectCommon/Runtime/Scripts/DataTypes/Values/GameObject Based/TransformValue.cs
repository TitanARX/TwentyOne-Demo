using UnityEngine;

namespace Com.FastEffect.DataTypes
{
    [CreateAssetMenu(menuName = "Fast Effect / Data Types / Transform Value")]
    [System.Serializable]
    public class TransformValue : ComponentValue<Transform>
    {

    }

    [System.Serializable]
    public class TransformReference : ComponentReference<Transform>
    {
        public TransformReference()
        {
        }

        public TransformReference(Transform value) : base(value)
        {
            
        }
    }
}