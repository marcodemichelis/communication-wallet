using WebApi.Models;

namespace WebApi.Services.SignalRHubs;

public interface ICommunicationHubServerNotifier
{
    Task NotifyRequestResultAsync(string username, Result result);
    Task NotifyMessageResultAsync(string username, string result);
    Task NotifyNewChangesForUser(string username);
    Task NotifyNewChangesForAll();
}