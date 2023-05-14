namespace WebApi.Services.Interfaces;

public interface IMessageQueue<T>
{
    Task<bool> Enqueue(T item);
    IEnumerable<T> Read(CancellationToken token);
}
