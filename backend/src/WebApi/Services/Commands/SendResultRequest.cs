using MediatR;
using WebApi.Models;

namespace WebApi.Services.Commands;

public record SendResultRequest(string ConnectionId, string UserName, Result Result)
    : NewRequest(ConnectionId, UserName, Guid.NewGuid().ToString()), IRequest
{
}
