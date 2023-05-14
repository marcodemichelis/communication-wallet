namespace WebApi.Services.Commands;


public record NewRequest(string ConnectionId, string UserName, string OperationId);
