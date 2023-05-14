using Interpreter;
using Interpreter.Recognizers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using WebApi.Services.Interfaces;

namespace WebApi.Services.SignalRHubs;

public class CommunicationHub : Hub
{
	private readonly IInterpreter _interpreter;
	private readonly IInterpreterDispatcher _dispatcher;
	private readonly IHttpContextAccessor _httpContextAccessor;
	public CommunicationHub(
		IInterpreterDispatcher interpreterDispatcher, 
		IInterpreter interpreter,
		IHttpContextAccessor httpContextAccessor)
	{
		_dispatcher = interpreterDispatcher;
		_interpreter = interpreter;
		_httpContextAccessor = httpContextAccessor;
	}


	public string GetConnectionId() => Context.ConnectionId;

	public Task BroadcastMessage(bool important, string message)
		 => Clients.All.SendAsync("newBroadcastMessagesReceived", important, message);

	public Task Echo(string username, string message)
		 => Clients.User(username).SendAsync("echo", username, $"{message} marco (echo from server)");

	public async Task AskToInterpreter(string message)
	{
		string connectionId = GetConnectionId();
		string username = _httpContextAccessor?.HttpContext?.Request?.Query?["username"] ?? "user-not-defined";
		await Task.Delay(1000);
		try
		{
			var result = await _interpreter.AskAsync(message);
			var dispatchResult = await _dispatcher.DispatchAsync(connectionId, username, result);
			await Clients.Caller.SendAsync("newInterpreterResponseReceived", dispatchResult.Message);
		}
		catch (RecognizerException ex)
		{
			await Clients.Caller.SendAsync("newInterpreterResponseReceived", ex.Message);
		}
		catch (Exception ex)
		{
			await Clients.Caller.SendAsync("newInterpreterResponseReceived", $"Si è verificato un errore generico: {ex.Message}");
		}
	}

	public Task EchoCaller(string name, string message)
		  => Clients.Caller.SendAsync("echo", name, $"{message} (echo from server)");


}

public class CustomUserIdProvider : IUserIdProvider
{
	readonly IHttpContextAccessor _httpContextAccessor;

	public CustomUserIdProvider(IHttpContextAccessor httpContextAccessor)
		 => _httpContextAccessor = httpContextAccessor;

	public string GetUserId(HubConnectionContext connection)
		 => _httpContextAccessor?.HttpContext?.Request?.Query?["username"] ?? "user-not-defined";
}
