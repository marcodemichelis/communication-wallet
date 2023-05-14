using MediatR;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Repositories.HistoryItem;
using WebApi.Services.Interfaces;
using WebApi.Services.SignalRHubs;

namespace WebApi.Services.Commands.Handlers;

public class ProcessLongReportRequestHandler : IRequestHandler<ProcessLongReportRequest>
{
    private readonly IMessageQueue<SendResultRequest> _queue;
    private readonly IAsyncActionItemRepository _repository;
    private readonly ICommunicationHubServerNotifier _notifier;

    public ProcessLongReportRequestHandler(IMessageQueue<SendResultRequest> queue, IAsyncActionItemRepository repository, ICommunicationHubServerNotifier notifier)
    {
        _queue = queue;
        _repository = repository;
        _notifier = notifier;
    }
    public async Task Handle(ProcessLongReportRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(3000);

        var item = await _repository.GetByOperationId(request.OperationId);
        var itemInProgress = item with { Status = ActionStatuses.InProgress };
        await _repository.Upsert(itemInProgress);
        await _notifier.NotifyNewChangesForUser(request.UserName);

        await Task.Delay(30000);

        var itemProcessed = item with { 
            Status = ActionStatuses.Processed,
            Message = $"Report generato per il cliente '{request.CustomerName}' <a target='_blank' href='[beBaseUrl]/api/communication/download-report?filename={request.CustomerName}.pdf'>download</a>"
        };
        await _repository.Upsert(itemProcessed);
        await _queue.Enqueue(new SendResultRequest(request.ConnectionId, request.UserName, new Result(ItemTypes.Request, "Long report has been generated <a>download<a/>")));
    }
}