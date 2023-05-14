using System.Reflection;

namespace Interpreter.EntityRecognizers
{
	public class DomainEntityRecognizer : BaseWordRecognizer, IEntityRecognizer
	{
		public Task<Entity[]> GetEntitiesAsync(string text)
		{
			List<Entity> entities = new List<Entity>();
			entities.AddRange(SearchFor_Words(text, "Skill", "modulo", "adesione"));
			entities.AddRange(SearchFor_Words(text, "Skill", "descrizione", "prodotto"));

			return Task.FromResult(entities.ToArray());
		}

	}

}
