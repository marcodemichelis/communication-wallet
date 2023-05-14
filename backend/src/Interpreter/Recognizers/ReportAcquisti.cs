using Interpreter.Recognizers.Result;
using Interpreter.Recognizers.Rule;
using System.Text.RegularExpressions;

namespace Interpreter.Recognizers
{
	public class ReportAcquisti : RecognizerBase, IRecognizer
	{
		public IEnumerable<EntityRule> Rules => new EntityRule[] {
			new EntityRule(EntityCategory.Skill) { Text = "report" },
			new EntityRule(EntityCategory.Skill) { Text = "acquisti" },
		};

		public RecognizerResult? GetRecognizerResult(IEnumerable<Entity> entities)
		{
			var result = CreateResult("", Rules, entities);
			if (result != null)
			{
				throw new RecognizerException("Mi dispiace, ma il report degli acquisti non è ancora disponibile.");
			}
			return result;
		}


	}

}
