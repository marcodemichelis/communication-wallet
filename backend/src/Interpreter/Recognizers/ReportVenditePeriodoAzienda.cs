using Interpreter.Recognizers.Result;
using Interpreter.Recognizers.Rule;
using System.Text.RegularExpressions;

namespace Interpreter.Recognizers
{
	public class ReportVenditeNoSoggettoNoPeriodo : RecognizerBase, IRecognizer
	{
		public IEnumerable<EntityRule> Rules => new EntityRule[] {
			new EntityRule(EntityCategory.Skill) { Text = "report" },
			new EntityRule(EntityCategory.Skill) { Text = "vendite" },
		};

		public RecognizerResult? GetRecognizerResult(IEnumerable<Entity> entities)
		{
			var result = CreateResult("", Rules, entities);
			if (result != null)
			{
				throw new RecognizerException("Per poter creare il report delle vendite ho bisogno del nome del cliente e di un range di date.");
			}
			return result;
		}

	}

}
