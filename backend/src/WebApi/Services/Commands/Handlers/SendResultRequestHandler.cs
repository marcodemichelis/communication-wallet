using MediatR;
using WebApi.Services.SignalRHubs;

namespace WebApi.Services.Commands.Handlers;

public class SendResultRequestHandler : IRequestHandler<SendResultRequest>
{
    private readonly ICommunicationHubServerNotifier _communicationHubServerNotifier;

    public SendResultRequestHandler(ICommunicationHubServerNotifier communicationHubServerNotifier)
        => _communicationHubServerNotifier = communicationHubServerNotifier;

    public Task Handle(SendResultRequest request, CancellationToken cancellationToken)
        => _communicationHubServerNotifier.NotifyNewChangesForUser(request.UserName);
    //        => _communicationHubServerNotifier.NotifyRequestResultAsync(request.UserName, request.Result);
}