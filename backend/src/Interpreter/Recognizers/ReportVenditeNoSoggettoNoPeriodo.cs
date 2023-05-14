using Interpreter.Recognizers.Result;
using Interpreter.Recognizers.Rule;
using System.Text.RegularExpressions;

namespace Interpreter.Recognizers
{
	public class ReportVenditePeriodoAzienda : RecognizerBase, IRecognizer
	{
		public IEnumerable<EntityRule> Rules => new EntityRule[] {
			new EntityRule(EntityCategory.Skill) { Text = "vendite" },
			new EntityRule(EntityCategory.Organization) {
				ExtractParams= GetNomeClienteParameter
			},
			new EntityRule(EntityCategory.DateTime){ Subcategory = "DateRange",
				ExtractParams= GetDateStartDateEndParameters
			}
		};

		public RecognizerResult? GetRecognizerResult(IEnumerable<Entity> entities) =>
			 CreateResult("ReportVendite", Rules, entities);

	}

}
