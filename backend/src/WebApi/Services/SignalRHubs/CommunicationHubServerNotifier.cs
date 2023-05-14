using Microsoft.AspNetCore.SignalR;
using WebApi.Models;

namespace WebApi.Services.SignalRHubs;

public class CommunicationHubServerNotifier : ICommunicationHubServerNotifier
{
    private readonly IHubContext<CommunicationHub> _hubContext;
    private const string NewRequestResultReceivedMethod = "newRequestResultReceived";
    private const string NewMessageResultReceivedMethod = "newMessageResultReceived";
    private const string NewChangesForUserReceivedMethod = "newChangesForUserReceived";

    public CommunicationHubServerNotifier(IHubContext<CommunicationHub> hubContext)
        => _hubContext = hubContext;

    public Task NotifyRequestResultAsync(string username, Result result)
        => _hubContext.Clients.User(username).SendAsync(NewRequestResultReceivedMethod, result);

    public Task NotifyMessageResultAsync(string username, string result)
        => _hubContext.Clients.User(username).SendAsync(NewMessageResultReceivedMethod, result);

    public Task NotifyNewChangesForUser(string username)
        => _hubContext.Clients.User(username).SendAsync(NewChangesForUserReceivedMethod);
    public Task NotifyNewChangesForAll()
        => _hubContext.Clients.All.SendAsync(NewChangesForUserReceivedMethod);
}