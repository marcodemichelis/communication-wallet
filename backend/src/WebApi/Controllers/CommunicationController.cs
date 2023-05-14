using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using WebApi.Services;
using WebApi.Services.Commands;
using WebApi.Services.Queries;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommunicationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CommunicationController(IMediator mediator, IWebHostEnvironment webHostEnvironment)
    {
        _mediator = mediator;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpPost("long-report")]
    public async Task<IActionResult> RequestLongReport([FromBody] EnqueueLongReportRequest request)
        => Ok(await _mediator.Send(request));

    [HttpPost("long-task")]
    public async Task<IActionResult> RequestLongTask([FromBody] EnqueueLongTaskRequest request)
        => Ok(await _mediator.Send(request));

    [HttpPost("enable-contextual-info-push")]
    public Task<IActionResult> EnableContextualInfoPush()
    {
        BroadcastNewsHostedService.PushItemsEnabled = true;
        return Task.FromResult<IActionResult>(Ok());
    }
    [HttpPost("disable-contextual-info-push")]
    public Task<IActionResult> DisableContextualInfoPush()
    {
        BroadcastNewsHostedService.PushItemsEnabled = false;
        return Task.FromResult<IActionResult>(Ok());
    }

    [HttpGet("download-report")]
    public Task<IActionResult> DownloadReport([FromQuery] string filename)
    {
        var localFilePath = $"{_webHostEnvironment.ContentRootPath}/Data/code-games-report.pdf";
        return Task.FromResult<IActionResult>(File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream", filename));
    }

    [HttpGet("get-items-by-user")]
    public async Task<IActionResult> GetItems(string username)
        => Ok(await _mediator.Send(new GetItemsByUserQuery(username)));
}
