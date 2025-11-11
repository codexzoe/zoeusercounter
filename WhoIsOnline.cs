using System.Security.Claims;

namespace ZoeUserCounter;
public class WhoIsOnline
{
    private readonly Dictionary<string, string> _connections = new();
    // Key: ConnectionId, Value: Username

    public event Action? OnUsersChanged;

    public IReadOnlyCollection<string> OnlineUsers
    {
        get
        {
            lock (_connections)
            {
                return _connections.Values.Distinct().ToList().AsReadOnly();
            }
        }
    }

    public void Add(string connectionId, ClaimsPrincipal user)
    {
        string username;

        if (user == null)
        {
            username = "Anonim";
        }
        else if (user.Identity?.IsAuthenticated == true)
        {
            username = user.Identity?.Name ?? "Anonim";
        }
        else
        {
            username = "Anonim";
        }

        lock (_connections)
        {
            _connections[connectionId] = username;
        }

        OnUsersChanged?.Invoke();
    }

    public void Remove(string connectionId)
    {
        lock (_connections)
        {
            _connections.Remove(connectionId);
        }

        OnUsersChanged?.Invoke();
    }
}
