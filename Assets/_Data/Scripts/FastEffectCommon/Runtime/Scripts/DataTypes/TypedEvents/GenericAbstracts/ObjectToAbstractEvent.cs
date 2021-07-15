using System;
using UnityEngine;
using UnityEngine.Events;
namespace Com.FastEffect.Events
{
    public abstract class ObjectToAbstractEvent<T> : MonoBehaviour
    {
        [Serializable]
        public class ObjectUEvent : UnityEvent<object>
        {

        }
        protected abstract UnityEvent<T> OutputEvent { get; }
        public void Convert(object obj)
        {
            OutputEvent.Invoke((T)obj);
        }
    }
}