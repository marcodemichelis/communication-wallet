using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.EntityRecognizers
{
	public class BaseWordRecognizer
	{
		protected Entity[] SearchFor_Words(string text, string category, params string[] words)
		{
			int[] indexes = new int[words.Length];
			//Find word indexes
			for (int i = 0; i < words.Length; i++)
			{
				var idx = text.IndexOf(words[i], StringComparison.InvariantCultureIgnoreCase);
				if (idx < 0) return Array.Empty<Entity>();
				indexes[i] = idx;
			}

			List<Entity> entities = new List<Entity>();
			for (int i = 0; i < words.Length; i++)
			{
				entities.Add(new Entity()
				{
					Category = category,
					Text = text.Substring(indexes[i], words[i].Length),
					Offset = indexes[i],
					Score = 1
				});
			}
			return entities.ToArray();

		}

	}
}
