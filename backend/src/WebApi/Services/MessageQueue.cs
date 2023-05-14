using System.Collections.Concurrent;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

public class MessageQueue<T> : IMessageQueue<T>
{
    readonly BlockingCollection<T> _queue;

    public MessageQueue()
    {
        _queue = new BlockingCollection<T>(new ConcurrentQueue<T>());
    }
    public IEnumerable<T> Read(CancellationToken token)
    {
        foreach (var item in _queue.GetConsumingEnumerable(token))
            yield return item;
    }

    public Task<bool> Enqueue(T item)
        => Task.FromResult(_queue.TryAdd(item));
}