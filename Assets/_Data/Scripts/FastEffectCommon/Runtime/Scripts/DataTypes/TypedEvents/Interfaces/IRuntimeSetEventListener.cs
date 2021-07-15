using System.Collections.Generic;
namespace Com.FastEffect.Events
{
    
    public interface IRuntimeSetEventListener<T>
    {
        void OnEnable();
        void OnDisable();
        void OnAddRaised(List<T> args);
        void OnRefreshRaised(List<T> args);
        void OnItemChanged(int index);
    }
}