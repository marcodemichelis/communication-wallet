using Interpreter.Recognizers.Result;
using Interpreter.Recognizers.Rule;

namespace Interpreter.Recognizers
{
	public interface IRecognizer
	{
		IEnumerable<EntityRule> Rules { get; }
		RecognizerResult? GetRecognizerResult(IEnumerable<Entity> entities);
	}
}
