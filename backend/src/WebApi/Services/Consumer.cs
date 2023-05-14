using MediatR;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

public class Consumer<T> : IConsumer<T> where T : IRequest
{
    private readonly IMediator _mediator;

    public Consumer(IMediator mediator)
        => _mediator = mediator;
    public Task ConsumeAsync(T item)
        => _mediator.Send(item);
}