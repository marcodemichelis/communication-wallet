using MediatR;

namespace WebApi.Services.Commands;

public record ProcessLongTaskRequest(string ConnectionId, string UserName, string CustomerName, string OperationId = null!)
    : EnqueueLongTaskRequest(ConnectionId, UserName, CustomerName, OperationId), IRequest;
