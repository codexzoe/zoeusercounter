namespace ZoeUserCounter;
public class UserTracker
{
    private readonly HashSet<string> _connections = new();

    public event Action? OnCountChanged;

    public int OnlineCount => _connections.Count;

    public void Add(string connectionId)
    {
        lock (_connections)
        {
            if (_connections.Add(connectionId))
                NotifyChanged();
        }
    }

    public void Remove(string connectionId)
    {
        lock (_connections)
        {
            if (_connections.Remove(connectionId))
                NotifyChanged();
        }
    }

    private void NotifyChanged() => OnCountChanged?.Invoke();
}
