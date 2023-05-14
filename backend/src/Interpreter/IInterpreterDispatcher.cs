using Interpreter.Recognizers.Result;

namespace WebApi.Services.Interfaces
{
	public interface IInterpreterDispatcher
	{
		Task<DispatchResult> DispatchAsync(string connectionId, string username, RecognizerResult result);
	}
}
