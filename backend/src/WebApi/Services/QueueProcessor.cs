using MediatR;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

public class QueueProcessor<T> : IQueueProcessor<T> where T : IRequest
{
    private readonly IMessageQueue<T> _queue;
    private readonly IConsumer<T> _consumer;
    readonly CancellationTokenSource _tokenSource;

    public QueueProcessor(IMessageQueue<T> queue, IConsumer<T> consumer)
    {
        _queue = queue;
        _consumer = consumer;
        _tokenSource = new CancellationTokenSource();
    }
    public Task Start()
    {
        Task.Run(() =>
        {
            foreach (var item in _queue.Read(_tokenSource.Token))
            {
                Task.Run(() => _consumer.ConsumeAsync(item));
            }
        }, _tokenSource.Token);
        return Task.CompletedTask;
    }
    public Task Stop()
    {
        _tokenSource.Cancel();
        return Task.CompletedTask;
    }
}
