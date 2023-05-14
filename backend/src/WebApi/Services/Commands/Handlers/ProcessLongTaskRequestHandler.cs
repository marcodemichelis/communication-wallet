using MediatR;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Repositories.HistoryItem;
using WebApi.Services.Interfaces;
using WebApi.Services.SignalRHubs;

namespace WebApi.Services.Commands.Handlers;

public class ProcessLongTaskRequestHandler : IRequestHandler<ProcessLongTaskRequest>
{
    private readonly IMessageQueue<SendResultRequest> _queue;
    private readonly IAsyncActionItemRepository _repository;
    private readonly ICommunicationHubServerNotifier _notifier;

    public ProcessLongTaskRequestHandler(IMessageQueue<SendResultRequest> queue, IAsyncActionItemRepository repository, ICommunicationHubServerNotifier notifier)
    {
        _queue = queue;
        _repository = repository;
        _notifier = notifier;
    }
    public async Task Handle(ProcessLongTaskRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(3000);

        var item = await _repository.GetByOperationId(request.OperationId);
        var itemInProgress = item with { Status = ActionStatuses.InProgress };
        await _repository.Upsert(itemInProgress);
        await _notifier.NotifyNewChangesForUser(request.UserName);

        await Task.Delay(60000);

        var itemProcessed = item with
        {
            Status = ActionStatuses.Processed,
            Message = $"Task elaborato per il cliente '{request.CustomerName}'"
        };


        await _repository.Upsert(itemProcessed);
        await _queue.Enqueue(new SendResultRequest(request.ConnectionId, request.UserName, new Result(ItemTypes.Request, "Long TASK has been processed successfully")));
    }
}
