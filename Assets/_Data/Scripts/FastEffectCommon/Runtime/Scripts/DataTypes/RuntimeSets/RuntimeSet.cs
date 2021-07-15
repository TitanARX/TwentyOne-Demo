using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Com.FastEffect.Events;

namespace Com.FastEffect.DataTypes
{
    [Serializable]
    // [CreateAssetMenu(menuName = "Fast Effect / Runtime Sets / <T> Set")]
    public abstract class RuntimeSet<T> : ScriptableObject, IList<T>, IRuntimeSetEvent<T>
    {
        [Multiline]
        public  string DeveloperDescription = "";

        [SerializeField]
        private List<T> m_items = new List<T>();
        public List<T> Items
        {
            get
            {
                return m_items;
            }
        }

        public int Count => m_items.Count;

        // shrug
        public bool IsReadOnly => false;

        public T this[int index] 
        { 
            get => m_items[index]; 
            set {
                m_items[index] = value;
                RaiseItemChanged(index);
                }
        }

        public void SetNewList(IEnumerable<T> newList)
        {
            if(newList == null)
            {
                // uh...
                return;
            }
            m_items = new List<T>(newList);
            RaiseRefresh();
        }
        public void SetListWithoutEvent(IEnumerable<T> newList)
        {
            if(newList == null)
            {
                return;
            }
            m_items = new List<T>(newList);
        }

        protected readonly List<IRuntimeSetEventListener<T>> _listeners = new List<IRuntimeSetEventListener<T>>();
        public void RaiseAdd(List<T> args)
        {
            foreach (IRuntimeSetEventListener<T> l in _listeners)
            {
                l.OnAddRaised(args);
            }
        }
        public void RaiseRefresh()
        {
            foreach (IRuntimeSetEventListener<T> l in _listeners)
            {
                l.OnRefreshRaised(Items);
            }
        }
        public void RaiseItemChanged(int index)
        {
            foreach(IRuntimeSetEventListener<T> l in _listeners)
            {
                l.OnItemChanged(index);
            }
        }

        public void Add(T thing)
        {
            m_items.Add(thing);
            List<T> e = new List<T>();
            e.Add(thing);
            RaiseAdd(e);
        }
        public void AddWithoutTrigger(T thing)
        {
            m_items.Add(thing);
        }
        public void AddRange(List<T> things)
        {
            m_items.AddRange(things);
            RaiseAdd(things);
        }
        public void AddWithoutTrigger(List<T> things)
        {
            m_items.AddRange(things);
        }

        public int IndexOf(T item)
        {
            return m_items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            m_items.Insert(index, item);
            RaiseRefresh();
        }
        public void InsertWithoutTrigger(int index, T item)
        {
            m_items.Insert(index, item);
        }
        public void InsertRange(int index, IEnumerable<T> items)
        {
            m_items.InsertRange(index, items);
            RaiseRefresh();
        }
        public void InsertWithoutTrigger(int index, IEnumerable<T> items)
        {
            m_items.InsertRange(index, items);
        }
        public void Clear()
        {
            m_items.Clear();
            RaiseRefresh();
        }

        public bool Contains(T item)
        {
            return m_items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_items.CopyTo(array, arrayIndex);
            RaiseRefresh();
        }
        public void CopyTo(T[] array)
        {
            m_items.CopyTo(array);
            RaiseRefresh();
        }
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            m_items.CopyTo(index, array, arrayIndex, count);
            RaiseRefresh();
        }

        public bool RemoveItem(T item)
        {
            bool result = m_items.Remove(item);
            if (result)
            {
                RaiseRefresh();
            }
            return result;
        }
        public bool RemoveWithoutTrigger(T item)
        {
            return m_items.Remove(item);
        }

        bool ICollection<T>.Remove(T item)
        {
            bool result = m_items.Remove(item);
            if (result)
            {
                RaiseRefresh();
            }
            return result;
        }
        public void RemoveAt(int index)
        {
            m_items.RemoveAt(index);
            RaiseRefresh();
        }
        public void RemoveAtWithoutTrigger(int index)
        {
            m_items.RemoveAt(index);
        }
        public void RemoveRange(int index, int count)
        {
            m_items.RemoveRange(index, count);
            RaiseRefresh();
        }
        public void RemoveRangeWithoutTrigger(int index,int count)
        {
            m_items.RemoveRange(index, count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        protected IEnumerator GetEnumerator1()
        {
            return this.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }

        public void Subscribe(IRuntimeSetEventListener<T> t)
        {
            if (!_listeners.Contains(t))
            {
                _listeners.Add(t);
            }
        }

        public bool Unsubscribe(IRuntimeSetEventListener<T> t)
        {
            return _listeners.Remove(t);
        }

        public static implicit operator List<T>(RuntimeSet<T> reference)
        {
            return reference.Items;
        }
    }
    [Serializable]
    public abstract class RuntimeSetReference<T> : IList<T>,ISerializationCallbackReceiver
    {
        [SerializeField]
        protected bool m_useConstant = true;
        [SerializeField]
        protected List<T> m_constantList = new List<T>();
        [SerializeReference]
        protected ScriptableObject m_refrenceObject = null;
        protected RuntimeSet<T> m_variableList = null;
        public void OnBeforeSerialize()
        {
            m_refrenceObject = m_variableList;
        }

        public void OnAfterDeserialize()
        {
            if(m_refrenceObject is RuntimeSet<T>)
            {
                m_variableList = (RuntimeSet<T>)m_refrenceObject;
            }
            else if(m_refrenceObject == null)
            {
                m_useConstant = true;
                m_variableList = null;
            }
            else
            {
                m_refrenceObject = null;
            }
        }

        public List<T> Items
        {
            get
            {
                if (m_useConstant)
                {
                    return m_constantList;
                }
                else
                {
                    return m_variableList.Items;
                }
            }
        }
        public void SetNewList(IEnumerable<T> newList)
        {
            if (m_useConstant)
            {
                m_constantList = new List<T>(newList);
            }
            else
            {
                m_variableList.SetNewList(newList);
            }
        }
        public void SetVarListWithoutEvent(IEnumerable<T> newList)
        {
            if (!m_useConstant)
            {
                m_variableList.SetListWithoutEvent(newList);
            }
            else
            {
                m_constantList = new List<T>(newList);
            }
        }

        public T this[int index]
        {
            get
            {
                if(m_useConstant)
                {
                    return m_constantList[index];
                }
                else
                {
                    return m_variableList[index];
                }
            }
            set
            {
                if (m_useConstant)
                {
                    m_constantList[index] = value;
                }
                else
                {
                    m_variableList[index] = value;
                }
            }
        }

        public int Count => Items.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (m_useConstant)
            {
                m_constantList.Add(item);
            }
            else
            {
                m_variableList.Add(item);
            }
        }
        public void AddWithoutTrigger(T item)
        {
            if (m_useConstant)
            {
                m_constantList.Add(item);
            }
            else
            {
                m_variableList.AddWithoutTrigger(item);
            }
        }

        public void Clear()
        {
            if(m_useConstant)
            {
                m_constantList.Clear();
            }
            else
            {
                m_variableList.Clear();
            }
        }

        public bool Contains(T item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (m_useConstant)
            {
                m_constantList.CopyTo(array, arrayIndex);
            }
            else
            {
                m_variableList.CopyTo(array, arrayIndex);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (m_useConstant)
            {
                return m_constantList.GetEnumerator();
            }
            else
            {
                return m_variableList.GetEnumerator();
            }
        }

        public int IndexOf(T item)
        {
            return Items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (m_useConstant)
            {
                m_constantList.Insert(index, item);
            }
            else
            {
                m_variableList.Insert(index, item);
            }
        }
        public void InsertWithoutTrigger(int index, T item)
        {
            if (m_useConstant)
            {
                m_constantList.Insert(index, item);
            }
            else
            {
                m_variableList.InsertWithoutTrigger(index, item);
            }
        }
        public void InsertRange(int index, IEnumerable<T> items)
        {
            if (m_useConstant)
            {
                m_constantList.InsertRange(index, items);
            }
            else
            {
                m_variableList.InsertRange(index, items);
            }
        }
        public void InsertWithoutTrigger(int index, IEnumerable<T> items)
        {
            if (m_useConstant)
            {
                m_constantList.InsertRange(index, items);
            }
            else
            {
                m_variableList.InsertWithoutTrigger(index, items);
            }
        }

        public bool Remove(T item)
        {
            if (m_useConstant)
            {
                return m_constantList.Remove(item);
            }
            else
            {
                return m_variableList.RemoveItem(item);
            }
        }
        public bool RemoveWithoutTrigger(T item)
        {
            if (m_useConstant)
            {
                return m_constantList.Remove(item);
            }
            else
            {
                return m_variableList.RemoveWithoutTrigger(item);
            }
        }

        public void RemoveAt(int index)
        {
            if (m_useConstant)
            {
                m_constantList.RemoveAt(index);
            }
            else
            {
                m_variableList.RemoveAt(index);
            }
        }
        public void RemoveAtWithoutTrigger(int index)
        {
            if (m_useConstant)
            {
                m_constantList.RemoveAt(index);
            }
            else
            {
                m_variableList.RemoveAtWithoutTrigger(index);
            }
        }

        public void RemoveRange(int index, int count)
        {
            if (m_useConstant)
            {
                m_constantList.RemoveRange(index, count);
            }
            else
            {
                m_variableList.RemoveRange(index, count);
            }
        }
        public void RemoveRangeWithoutTrigger(int index, int count)
        {
            if (m_useConstant)
            {
                m_constantList.RemoveRange(index, count);
            }
            else
            {
                m_variableList.RemoveRangeWithoutTrigger(index, count);
            }
        }

        protected IEnumerator GetEnumerator1()
        {
            return this.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }

        public static implicit operator List<T>(RuntimeSetReference<T> reference)
        {
            return reference.Items;
        }
    }
}