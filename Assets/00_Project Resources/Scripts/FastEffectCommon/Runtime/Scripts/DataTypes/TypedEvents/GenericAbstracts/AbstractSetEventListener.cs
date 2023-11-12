using Com.FastEffect.DataTypes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Com.FastEffect.Events
{

    public abstract class AbstractSetEventListener<T> : MonoBehaviour, IRuntimeSetEventListener<T>
    {
        protected abstract RuntimeSet<T> ObservedSet { get; }
        protected abstract UnityEvent<List<T>> OnSetAdd { get; }
        protected abstract UnityEvent<List<T>> OnSetRefresh { get; }
        protected abstract UnityEvent<int> OnSetItemChanged { get; }
        public void OnRefreshRaised(List<T> args)
        {
            OnSetRefresh.Invoke(new List<T>(args));
        }
        public void OnAddRaised(List<T> args)
        {

            OnSetAdd.Invoke( new List<T>(args));
        }
        public void OnItemChanged(int index)
        {
            OnSetItemChanged.Invoke(index);
        }
        public void OnEnable()
        {
            ObservedSet.Subscribe(this);
        }

        public void OnDisable()
        {
            ObservedSet.Unsubscribe(this);
        }
    }
}