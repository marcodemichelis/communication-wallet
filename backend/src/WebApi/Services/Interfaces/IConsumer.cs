using MediatR;

namespace WebApi.Services.Interfaces;

public interface IConsumer<T> where T : IRequest
{
    Task ConsumeAsync(T item);
}
