public class EventDispatcher
{
    public static EventDispatcher Instance => _instance ??= new EventDispatcher();
    private static EventDispatcher _instance;

    private readonly Dictionary<Type, dynamic> _events;

    public EventDispatcher()
    {
        _events = new Dictionary<Type, dynamic>();
    }

    public void Subscribe<T>(Action<T> callback)
    {
        var type = typeof(T);
        if (!_events.ContainsKey(type))
        {
            _events.Add(type, null);
        }

        _events[type] += callback;
    }

    public void Unsubscribe<T>(Action<T> callback)
    {
        var type = typeof(T);
        if (_events.ContainsKey(type))
        {
            _events[type] -= callback;
        }
    }

    public void Dispatch<T>(T signal)
    {
        var type = typeof(T);
        if (!_events.ContainsKey(type))
            return;
        _events[type](signal);
    }
}