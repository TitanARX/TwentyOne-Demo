using UnityEngine;
using UnityEngine.Events;
using Com.FastEffect.DataTypes;
namespace Com.FastEffect.Events
{
    

    public abstract class AbstractEventListener<T> : MonoBehaviour, IOneArgEventListener<T>
    {
        protected abstract AbstractValue<T> ValueEvent { get; }
        protected abstract UnityEvent<T> Response { get; }

        public void OnEnable()
        {
            if (ValueEvent != null)
            {
                ValueEvent.Subscribe(this);
            }
        }

        public void OnDisable()
        {
            if (ValueEvent != null)
            {
                ValueEvent.Unsubscribe(this);
            }
        }

        virtual public void OnEventRaised(T arg)
        {
            Response.Invoke(arg);
        }
    }
}