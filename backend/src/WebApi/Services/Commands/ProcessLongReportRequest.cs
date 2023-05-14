using MediatR;

namespace WebApi.Services.Commands;

public record ProcessLongReportRequest(string ConnectionId, string UserName, string CustomerName, string OperationId = null!)
    : EnqueueLongReportRequest(ConnectionId, UserName, CustomerName, OperationId), IRequest;
