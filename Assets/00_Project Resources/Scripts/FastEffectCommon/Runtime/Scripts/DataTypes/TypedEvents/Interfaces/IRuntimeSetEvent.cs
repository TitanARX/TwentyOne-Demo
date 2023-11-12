using System.Collections.Generic;
namespace Com.FastEffect.Events
{
    public interface IRuntimeSetEvent<T>
    {
        void RaiseRefresh();
        void RaiseAdd(List<T> args);
        void Subscribe(IRuntimeSetEventListener<T> t);
        bool Unsubscribe(IRuntimeSetEventListener<T> t);
    }
}