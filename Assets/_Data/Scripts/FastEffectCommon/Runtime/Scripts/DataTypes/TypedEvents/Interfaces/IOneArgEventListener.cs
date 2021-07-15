namespace Com.FastEffect.Events
{
    public interface IOneArgEventListener<T>
    {
        void OnEnable();
        void OnDisable();
        void OnEventRaised(T arg);
    }
}