using WebApi.Services.Commands;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

public class RequestHandlerHostedService : IHostedService
{
    private readonly IQueueProcessor<ProcessLongReportRequest> _longReportRequestProcessor;
    private readonly IQueueProcessor<ProcessLongTaskRequest> _longTaskRequestProcessor;
    private readonly IQueueProcessor<SendResultRequest> _sendResultRequestProcessor;

    public RequestHandlerHostedService(
        IQueueProcessor<ProcessLongReportRequest> longReportRequestProcessor,
        IQueueProcessor<ProcessLongTaskRequest> longTaskRequestProcessor,
        IQueueProcessor<SendResultRequest> sendResultRequestProcessor)
    {
        _longReportRequestProcessor = longReportRequestProcessor;
        _longTaskRequestProcessor = longTaskRequestProcessor;
        _sendResultRequestProcessor = sendResultRequestProcessor;
    }

    public Task StartAsync(CancellationToken cancellationToken)
        => Task.WhenAll(
                _longReportRequestProcessor.Start(),
                _longTaskRequestProcessor.Start(),
                _sendResultRequestProcessor.Start());

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(
                _longReportRequestProcessor.Stop(),
                _longTaskRequestProcessor.Stop(),
                _sendResultRequestProcessor.Stop());
    }
}
