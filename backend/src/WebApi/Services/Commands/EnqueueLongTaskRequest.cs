using MediatR;

namespace WebApi.Services.Commands;

public record EnqueueLongTaskRequest(string ConnectionId, string UserName, string CustomerName, string OperationId = null!)
    : NewRequest(ConnectionId, UserName, OperationId ?? Guid.NewGuid().ToString()), IRequest<bool>
{
}
