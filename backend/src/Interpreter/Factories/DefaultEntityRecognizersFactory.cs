using Interpreter.EntityRecognizers;

namespace Interpreter.Factories
{
	public class DefaultEntityRecognizersFactory : IEntityRecognizersFactory
	{
		public IEnumerable<IEntityRecognizer> GetEntityRecognizers()
		{
			return new List<IEntityRecognizer>()
			{
				new AzureAIEntityRecognizer(),
				//new DomainEntityRecognizer(),
				//new ProductEntityRecognizer()
			};
		}
	}

}
