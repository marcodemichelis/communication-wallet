using MediatR;
using WebApi.Repositories.HistoryItem;
using WebApi.Services.Interfaces;
using WebApi.Services.SignalRHubs;

namespace WebApi.Services.Commands.Handlers;

public class EnqueueLongTaskRequestHandler : IRequestHandler<EnqueueLongTaskRequest, bool>
{
    private readonly IAsyncActionItemRepository _asyncActionItemRepository;
    private readonly IMessageQueue<ProcessLongTaskRequest> _reportQueue;
    private readonly ICommunicationHubServerNotifier _notifier;

    public EnqueueLongTaskRequestHandler(IAsyncActionItemRepository asyncActionItemRepository, IMessageQueue<ProcessLongTaskRequest> reportQueue, ICommunicationHubServerNotifier notifier)
    {
        _asyncActionItemRepository = asyncActionItemRepository;
        _reportQueue = reportQueue;
        _notifier = notifier;
    }
    public async Task<bool> Handle(EnqueueLongTaskRequest request, CancellationToken cancellationToken)
    {
        var item = new Entities.AsyncActionItem(
                request.ConnectionId,
                request.UserName,
                request.OperationId,
                Entities.ItemTypes.Request,
                $"Richiesta esecuzione TASK per il cliente '{request.CustomerName}'",
                Entities.ActionStatuses.Enqueued,
                DateTime.Now);
        await _asyncActionItemRepository.Upsert(item);
        await _notifier.NotifyNewChangesForUser(request.UserName);
        return await _reportQueue.Enqueue(new ProcessLongTaskRequest(request.ConnectionId, request.UserName, request.CustomerName, request.OperationId));
    }
}
