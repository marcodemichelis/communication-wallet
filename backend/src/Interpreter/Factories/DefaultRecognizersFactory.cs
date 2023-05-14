using Interpreter.EntityRecognizers;
using Interpreter.Recognizers;

namespace Interpreter.Factories
{
	public class DefaultRecognizersFactory : IRecognizersFactory
	{
		public IEnumerable<IRecognizer> GetRecognizers()
		{
			return new List<IRecognizer>()
			{
				new ReportVenditePeriodoAzienda(),
				new ReportVenditePeriodoSoggetto(),
				new ReportAcquisti(),
				new ReportVenditeNoSoggetto(),
				new ReportVenditeNoPeriodo1(),
				new ReportVenditeNoPeriodo2(),
				new ReportVenditeNoSoggettoNoPeriodo()
			};
		}
	}

}
