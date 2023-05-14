namespace WebApi.Services.Interfaces
{
    public interface IQueueProcessor<T>
    {
        Task Start();
        Task Stop();
    }
}