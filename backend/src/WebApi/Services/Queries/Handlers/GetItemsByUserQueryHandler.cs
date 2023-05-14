using MediatR;
using WebApi.Entities;
using WebApi.Repositories.HistoryItem;

namespace WebApi.Services.Queries.Handlers;

public class GetItemsByUserQueryHandler : IRequestHandler<GetItemsByUserQuery, IEnumerable<AsyncActionItem>>
{
    private readonly IAsyncActionItemRepository _asyncActionItemRepository;

    public GetItemsByUserQueryHandler(IAsyncActionItemRepository asyncActionItemRepository)
    {
        _asyncActionItemRepository = asyncActionItemRepository;
    }

    public Task<IEnumerable<AsyncActionItem>> Handle(GetItemsByUserQuery request, CancellationToken cancellationToken)
        => _asyncActionItemRepository.ListByUsername(request.Username);
}