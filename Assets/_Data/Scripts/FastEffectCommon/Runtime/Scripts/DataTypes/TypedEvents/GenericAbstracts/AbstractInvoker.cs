using UnityEngine;
using UnityEngine.Events;
namespace Com.FastEffect.Events
{

    public abstract class AbstractInvoker<T> : MonoBehaviour
    {
        
        public abstract T Value { get; set; }
        protected abstract UnityEvent<T> InvokedFunctions { get; }
        public void Invoke()
        {
            InvokedFunctions.Invoke(Value);
        }
        public void Invoke(T value)
        {
            InvokedFunctions.Invoke(value);
        }
    }
}