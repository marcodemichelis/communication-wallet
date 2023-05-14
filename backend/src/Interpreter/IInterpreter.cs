using Interpreter.Recognizers.Result;

namespace Interpreter
{
	public interface IInterpreter
	{
		public Task<RecognizerResult> AskAsync(string message);
	}
}
