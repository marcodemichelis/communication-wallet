using Interpreter;
using Interpreter.EntityRecognizers;
using Interpreter.Factories;
using Interpreter.Recognizers;
using Interpreter.Recognizers.Result;
using Moq;

namespace InterpreterTest
{
	 public class JohnTitorTests
	{

		private readonly JohnTitor _johnTitor;

		public JohnTitorTests() {
			var mockEntityRecognizersFactory = new Mock<IEntityRecognizersFactory>();
			List<IEntityRecognizer> entityRecognizers = new()
			{
				new FakeAzureAIEntityRecognizer(),
				//new AzureAIEntityRecognizer(),
			};
			mockEntityRecognizersFactory.Setup(x => x.GetEntityRecognizers()).Returns(entityRecognizers);

			var mockRecognizersFactory = new Mock<IRecognizersFactory>();
			List<IRecognizer> recognizers = new List<IRecognizer>
			{
				new ReportVenditePeriodoAzienda(),
				new ReportVenditePeriodoSoggetto(),
			};
			mockRecognizersFactory.Setup(x => x.GetRecognizers()).Returns(recognizers);
			_johnTitor = new JohnTitor(mockEntityRecognizersFactory.Object, mockRecognizersFactory.Object);

		}

		[Fact]
		public async Task AskWork_ShouldReturn_ANotNullResult()
		{
			var result = await _johnTitor.AskAsync("test");
			Assert.NotNull(result);
			Assert.Equal(RecognizerResult.Empty, result);
		}

		[Fact]
		public async Task GetEntities_ShouldReturn_Entities()
		{
			var result = await _johnTitor.GetEntitiesFromRecognizersAsync("test");
			Assert.NotNull(result);
		}

		[Fact]
		public async Task TestCase_ReportVenditePeriodoAzienda_OperationName()
		{
			string request = "Vorrei il report delle vendite della ditta Spendox spa dal 1/1/2023 al 30/06/2023";
			
			var result = await _johnTitor.AskAsync(request);
			
			Assert.NotNull(result);
			Assert.NotEqual(RecognizerResult.Empty, result);
			Assert.Equal("ReportVendite", result.OperationName);
			
			Assert.Contains(result.Parameters, (p) =>
				p.Name == "NomeCliente" &&
				p.Value != null &&
				p.Value is string &
				(string)p.Value == "Spendox spa"
			);

			Assert.Contains(result.Parameters, (p) =>
				p.Name == "DateStart" &&
				p.Value != null &&
				p.Value is DateTime &
				(DateTime)p.Value == new DateTime(2023, 1, 1)
			);

			Assert.Contains(result.Parameters, (p) =>
				p.Name == "DateEnd" &&
				p.Value != null &&
				p.Value is DateTime &
				(DateTime)p.Value == new DateTime(2023, 06, 30)
			);


		}

		[Fact]
		public async Task TestCase_ReportVenditePeriodoSoggetto_OperationName()
		{
			string request = "Vorrei il report delle vendite del cliente Giampetruzzi Flavio dal 1/1/2023 al 30/06/2023";

			var result = await _johnTitor.AskAsync(request);

			Assert.NotNull(result);
			Assert.NotEqual(RecognizerResult.Empty, result);
			Assert.Equal("ReportVendite", result.OperationName);

			Assert.Contains(result.Parameters, (p) =>
				p.Name == "NomeCliente" &&
				p.Value != null &&
				p.Value is string &
				(string)p.Value == "Giampetruzzi Flavio"
			);

			Assert.Contains(result.Parameters, (p) =>
				p.Name == "DateStart" &&
				p.Value != null &&
				p.Value is DateTime &
				(DateTime)p.Value == new DateTime(2023, 1, 1)
			);

			Assert.Contains(result.Parameters, (p) =>
				p.Name == "DateEnd" &&
				p.Value != null &&
				p.Value is DateTime &
				(DateTime)p.Value == new DateTime(2023, 06, 30)
			);


		}




	}
}