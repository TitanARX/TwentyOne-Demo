using System;
using System.Collections.Generic;
using UnityEngine;
using Com.FastEffect.Events;

namespace Com.FastEffect.DataTypes
{
    [Serializable]
    public abstract class AbstractValue<T> : ScriptableObject,IAbstractValue<T>//,ISerializationCallbackReceiver
    { 
        [Multiline]
        public string DeveloperDescription = "";

        [SerializeField]
        protected T m_value;
        public T Value
        {
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;
                Raise(m_value);
            }
        }
        public void SetWithoutTrigger(T value)
        {
            m_value = value;
        }

        protected readonly List<IOneArgEventListener<T>> m_listeners = new List<IOneArgEventListener<T>>();
        /*
        [Header("Current Listening Objects")]
        [SerializeReference]
        private List<IOneArgEventListener<T>> m_listenersReadOnlyList = new List<IOneArgEventListener<T>>();
        public void OnBeforeSerialize()
        {
            m_listenersReadOnlyList = new List<IOneArgEventListener<T>>(m_listeners);
        }

        public void OnAfterDeserialize()
        {
            //nothing
        }
        */
        public void Raise(T arg)
        {
            for (int i = m_listeners.Count - 1; i >= 0; i--)
            {
                m_listeners[i].OnEventRaised(arg);
            }
        }

        public void Subscribe(IOneArgEventListener<T> t)
        {
            if (!m_listeners.Contains(t))
            {
                m_listeners.Add(t);
            }
        }

        public void Unsubscribe(IOneArgEventListener<T> t)
        {
            if (m_listeners.Contains(t))
            {
                m_listeners.Remove(t);
            }
        }

        public static implicit operator T(AbstractValue<T> value)
        {
            return value.Value;
        }
    }

    [Serializable]
    public abstract class AbstractReference<T>:ISerializationCallbackReceiver
    {
        [SerializeField]
        protected bool m_useConstant = true;
        [SerializeField]
        protected T m_constantValue = default;
        [SerializeField]
        protected ScriptableObject m_referenceObject = null;
        public void OnBeforeSerialize()
        {
            m_referenceObject = m_variableObject;
        }

        public void OnAfterDeserialize()
        {
            if(m_referenceObject is AbstractValue<T>)
            {
                m_variableObject = (AbstractValue<T>)m_referenceObject;
            }
            else
            {
                m_referenceObject = null;
            }
        }

        protected AbstractValue<T> m_variableObject = null;

        public AbstractReference() { }
        public AbstractReference(T value)
        {
            m_useConstant = true;
            m_constantValue = value;
        }

        public T Value
        {
            get
            {
                if (m_useConstant)
                {
                    return m_constantValue;
                }
                else
                {
                    return m_variableObject.Value;
                }
            }
            set
            {
                // updating the constant value anyway just for easier display reasons
                m_constantValue = value;

                if (!m_useConstant)
                {
                    m_variableObject.Value = value;
                }
            }
        }

        public void SetWithoutTrigger(T value)
        {
            if(m_useConstant)
            {
                m_constantValue = value;
            }
            else
            {
                m_variableObject.SetWithoutTrigger(value);
            }
        }

        // not sure what this does, just kinda copied it from floatreference
        public static implicit operator T(AbstractReference<T> reference)
        {
            return reference.Value;
        }
    }
}