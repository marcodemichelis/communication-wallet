using System.Collections.Concurrent;
using WebApi.Repositories.HistoryItem;

namespace WebApi.Repositories;

public class AsyncActionItemRepository : IAsyncActionItemRepository
{
    private readonly ConcurrentDictionary<string, Entities.AsyncActionItem> _items;

    public AsyncActionItemRepository()
        => _items = new ConcurrentDictionary<string, Entities.AsyncActionItem>(StringComparer.OrdinalIgnoreCase);

    public Task<Entities.AsyncActionItem> GetByOperationId(string operationId)
        => Task.FromResult(_items.FirstOrDefault(i => i.Key == operationId).Value);

    public Task<IEnumerable<Entities.AsyncActionItem>> ListByUsername(string username)
        => Task.FromResult<IEnumerable<Entities.AsyncActionItem>>(
            _items
                .Select(i => i.Value)
                .Where(i =>
                    string.Equals(i.UserName, username, StringComparison.InvariantCultureIgnoreCase)
                    || string.IsNullOrWhiteSpace(i.UserName)
                )
                .OrderByDescending(i => i.InsertDate)
                .ToList());

    public Task Upsert(Entities.AsyncActionItem item)
    {
        _items.AddOrUpdate(item.OperationId, item, (k, oldValue) => item);
        return Task.CompletedTask;
    }
}
