
using Interpreter.Recognizers;

namespace Interpreter.Factories
{
    public interface IRecognizersFactory
	{
        IEnumerable<IRecognizer> GetRecognizers();
    }

}
