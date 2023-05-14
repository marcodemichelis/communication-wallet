namespace WebApi.Repositories.HistoryItem;

public interface IAsyncActionItemRepository
{
    Task<IEnumerable<Entities.AsyncActionItem>> ListByUsername(string username);
    Task<Entities.AsyncActionItem> GetByOperationId(string operationId);
    Task Upsert(Entities.AsyncActionItem item);
}
