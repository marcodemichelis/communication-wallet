
namespace Interpreter.EntityRecognizers
{
    public interface IEntityRecognizer
    {
        public Task<Entity[]> GetEntitiesAsync(string text);
    }

}
