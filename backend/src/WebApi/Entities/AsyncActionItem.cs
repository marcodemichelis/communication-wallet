using WebApi.Services.Commands;

namespace WebApi.Entities;

public record AsyncActionItem(string ConnectionId, string UserName, string OperationId, ItemTypes ItemType, string Message, ActionStatuses Status, DateTime InsertDate)
    : NewRequest(ConnectionId, UserName, OperationId);


