using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ZoeUserCounter;
public class UserTrackingCircuitHandler : CircuitHandler
{
    private readonly UserTracker _tracker;
    private readonly WhoIsOnline _whoIsOnline;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserTrackingCircuitHandler(UserTracker tracker, WhoIsOnline whoIsOnline, IHttpContextAccessor httpContextAccessor)
    {
        _tracker = tracker;
        _whoIsOnline = whoIsOnline;
        _httpContextAccessor = httpContextAccessor;
    }

    public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _tracker.Add(circuit.Id);

        // HttpContext üzerinden user almaya çalış
        ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
        if (user == null || !user.Identity?.IsAuthenticated == true)
        {
            // fallback: anonim
            _whoIsOnline.Add(circuit.Id, null);
        }
        else
        {
            _whoIsOnline.Add(circuit.Id, user);
        }

        return Task.CompletedTask;
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _tracker.Remove(circuit.Id);
        _whoIsOnline.Remove(circuit.Id);
        return Task.CompletedTask;
    }
}
