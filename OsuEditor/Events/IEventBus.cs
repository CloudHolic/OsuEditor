namespace OsuEditor.Events
{
    public interface IEventBus
    {
        void RegisterHandler(object subscriber);
        void UnregisterHandler(object subscriber);
        void Publish<TEventType>(TEventType e);
    }
}
