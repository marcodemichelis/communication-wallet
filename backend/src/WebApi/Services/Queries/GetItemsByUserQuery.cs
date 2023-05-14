using MediatR;
using WebApi.Entities;

namespace WebApi.Services.Queries;

public record GetItemsByUserQuery(string Username) : IRequest<IEnumerable<AsyncActionItem>>;
