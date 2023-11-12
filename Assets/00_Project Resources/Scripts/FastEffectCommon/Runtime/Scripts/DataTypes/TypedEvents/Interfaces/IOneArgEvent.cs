namespace Com.FastEffect.Events
{
    public interface IOneArgEvent<T>
    {
        void Raise(T arg);
        void Subscribe(IOneArgEventListener<T> t);
        void Unsubscribe(IOneArgEventListener<T> t);
    }
    public interface IAbstractValue<T> : IOneArgEvent<T>
    {
        T Value { get; set; }
        void SetWithoutTrigger(T value);
    }

}