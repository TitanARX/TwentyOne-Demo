using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.FastEffect.DataTypes
{
    [System.Serializable]
    public class GameObjectValue : AbstractValue<GameObject>
    {
        
    }
    [System.Serializable]
    public class GameObjectReference : AbstractReference<GameObject>
    {
        public GameObjectReference()
        {
        }

        public GameObjectReference(GameObject value) : base(value)
        {

        }
    }
    [System.Serializable]
    public abstract class ComponentValue<T> : AbstractValue<T> where T : Component
    {
        public void SetValueFromGameObject(GameObject sourceObject)
        {
            if (sourceObject.TryGetComponent<T>(out T got))
            {
                Value = got;
            }
        }

        public void RaiseFromGameObject(GameObject sourceObject)
        {
            if(sourceObject.TryGetComponent<T>(out T got))
            {
                Raise(got);
            }
        }
        public void SetValueFromGameObjectWithoutTrigger(GameObject sourceObject)
        {
            if (sourceObject.TryGetComponent<T>(out T got))
            {
                SetWithoutTrigger(got);
            }
        }
    }
    [System.Serializable]
    public abstract class ComponentReference<T> : AbstractReference<T> where T : Component
    {
        public ComponentReference()
        {

        }
        public ComponentReference(T value) : base(value)
        {

        }
        public void SetValueFromGameObject(GameObject sourceObject)
        {
            if (sourceObject.TryGetComponent<T>(out T got))
            {
                Value = got;
            }
        }
        public new void OnAfterDeserialize()
        {
            if(m_referenceObject is ComponentValue<T>)
            {
                m_variableObject = (ComponentValue<T>)m_referenceObject;
            }
            else
            {
                m_referenceObject = null;
            }
        }

        public void SetValueFromGameObjectWithoutTrigger(GameObject sourceObject)
        {
            if (sourceObject.TryGetComponent<T>(out T got))
            {
                SetWithoutTrigger(got);
            }
        }
    }
}