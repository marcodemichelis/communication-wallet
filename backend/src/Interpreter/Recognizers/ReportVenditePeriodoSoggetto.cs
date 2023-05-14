using Interpreter.Recognizers.Result;
using Interpreter.Recognizers.Rule;

namespace Interpreter.Recognizers
{
	public class ReportVenditePeriodoSoggetto : RecognizerBase, IRecognizer
	{
		public IEnumerable<EntityRule> Rules => new EntityRule[] {
			new EntityRule(EntityCategory.Skill) { Text = "report" },
			new EntityRule(EntityCategory.Skill) { Text = "vendite" },
			new EntityRule(EntityCategory.PersonType) { Text = "cliente"},
			new EntityRule(EntityCategory.Person) {
				ExtractParams= GetNomeClienteParameter
			},
			new EntityRule(EntityCategory.DateTime) { Subcategory = "DateRange",
				ExtractParams= GetDateStartDateEndParameters
			}
		};

		public RecognizerResult? GetRecognizerResult(IEnumerable<Entity> entities) =>
			CreateResult("ReportVendite", Rules, entities);
		
	}

}
