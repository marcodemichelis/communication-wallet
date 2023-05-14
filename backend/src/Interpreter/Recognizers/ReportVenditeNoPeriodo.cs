using Interpreter.Recognizers.Result;
using Interpreter.Recognizers.Rule;
using System.Text.RegularExpressions;

namespace Interpreter.Recognizers
{
	public class ReportVenditeNoPeriodo1 : RecognizerBase, IRecognizer
	{
		public IEnumerable<EntityRule> Rules => new EntityRule[] {
			new EntityRule(EntityCategory.Skill) { Text = "report" },
			new EntityRule(EntityCategory.Skill) { Text = "vendite" },
			new EntityRule(EntityCategory.Organization) {
				ExtractParams= GetNomeClienteParameter
			},
		};

		public RecognizerResult? GetRecognizerResult(IEnumerable<Entity> entities)
		{
			var result = CreateResult("", Rules, entities);
			if (result != null)
			{
				throw new RecognizerException("Per poter creare il report delle vendite ho bisogno anche del range di date.");
			}
			return result;
		}


	}

	public class ReportVenditeNoPeriodo2 : RecognizerBase, IRecognizer
	{
		public IEnumerable<EntityRule> Rules => new EntityRule[] {
			new EntityRule(EntityCategory.Skill) { Text = "report" },
			new EntityRule(EntityCategory.Skill) { Text = "vendite" },
			new EntityRule(EntityCategory.PersonType) { Text = "cliente"},
			new EntityRule(EntityCategory.Person) {
				ExtractParams= GetNomeClienteParameter
			},
		};

		public RecognizerResult? GetRecognizerResult(IEnumerable<Entity> entities)
		{
			var result = CreateResult("", Rules, entities);
			if (result != null)
			{
				throw new RecognizerException("Per poter creare il report delle vendite ho bisogno anche del range di date.");
			}
			return result;
		}


	}

}
