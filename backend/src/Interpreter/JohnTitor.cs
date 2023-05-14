using Interpreter.EntityRecognizers;
using Interpreter.Factories;
using Interpreter.Recognizers;
using Interpreter.Recognizers.Result;

namespace Interpreter
{
	public class JohnTitor : IInterpreter
	{

		private readonly IEnumerable<IEntityRecognizer> entityRecognizers;
		private readonly IEnumerable<IRecognizer> recognizers;
		public JohnTitor(
			IEntityRecognizersFactory EntityRecognizersFactory, 
			IRecognizersFactory RecognizersFactory)
		{
			entityRecognizers = EntityRecognizersFactory.GetEntityRecognizers();
			recognizers = RecognizersFactory.GetRecognizers();
		}

		public async Task<RecognizerResult> AskAsync(string text)
		{
			var entities = await GetEntitiesFromRecognizersAsync(text);

			foreach (var recognizer in recognizers)
			{
				var result = recognizer.GetRecognizerResult(entities);
				if (result != null)
				{
					return result;
				}
			}

			return RecognizerResult.Empty;
		}

		internal async Task<IEnumerable<Entity>> GetEntitiesFromRecognizersAsync(string text)
		{
			var tasks = entityRecognizers.Select(er => er.GetEntitiesAsync(text));
			var result = await Task.WhenAll(tasks);
			var entities = result.SelectMany(a => a);
			return entities;
		}
	}

}
