using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.EntityRecognizers
{
	public class ProductEntityRecognizer : BaseWordRecognizer, IEntityRecognizer
	{
		public Task<Entity[]> GetEntitiesAsync(string text)
		{
			List<Entity> entities = new List<Entity>();
			entities.AddRange(SearchFor_Words(text, "Product", "genya"));
			return Task.FromResult(entities.ToArray());
		}
	}
}
