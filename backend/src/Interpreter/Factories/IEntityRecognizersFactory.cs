using Interpreter.EntityRecognizers;

namespace Interpreter.Factories
{
    public interface IEntityRecognizersFactory
    {
        IEnumerable<IEntityRecognizer> GetEntityRecognizers();
    }

}
