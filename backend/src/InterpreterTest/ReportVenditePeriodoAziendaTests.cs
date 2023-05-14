using Interpreter;
using Interpreter.Recognizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterTest
{
	public class ReportVenditePeriodoAziendaTests
	{
		[Fact]
		public void ParameterRecognitionTest()
		{
			IRecognizer recognizer = new ReportVenditePeriodoAzienda();

			var entities = new Entity[] {
				new Entity() {Category = "Skill", Text="Vendite"},
				new Entity() {Category="Organization", Text="Splendox spa" },
				new Entity() {Category="DateTime", SubCategory = "DateRange", Text= "dal 1/1/2023 al 21/06/23"}
			};

			var result = recognizer.GetRecognizerResult(entities);

			Assert.NotNull(result);
			Assert.Equal("ReportVendite", result.OperationName);
			Assert.Contains(result.Parameters, (p) =>
				p.Name == "NomeCliente" &&
				p.Value != null &&
				p.Value is string &
				(string)p.Value == "Splendox spa"
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
				(DateTime)p.Value == new DateTime(2023, 06, 21)
			);


		}

		[Fact]
		public void ParameterRecognitionInvertionTest()
		{
			IRecognizer recognizer = new ReportVenditePeriodoAzienda();

			var entities = new Entity[] {
				new Entity() {Category = "Skill", Text="Vendite"},
				new Entity() {Category="Organization", Text="Splendox spa" },
				new Entity() {Category="DateTime", SubCategory = "DateRange", Text= "dal 21/06/23 al 1/1/2023"}
			};

			var result = recognizer.GetRecognizerResult(entities);

			Assert.NotNull(result);
			Assert.Equal("ReportVendite", result.OperationName);
			Assert.Contains(result.Parameters, (p) =>
				p.Name == "NomeCliente" &&
				p.Value != null &&
				p.Value is string &
				(string)p.Value == "Splendox spa"
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
				(DateTime)p.Value == new DateTime(2023, 06, 21)
			);


		}


		[Fact]
		public void DoesntMatchWhenMissingOrganization()
		{
			IRecognizer recognizer = new ReportVenditePeriodoAzienda();

			var entities = new Entity[] {
				new Entity() {Category = "Skill", Text="Vendite"},
				new Entity() {Category="DateTime", SubCategory = "DateRange", Text= "dal 21/06/23 al 1/1/2023"}
			};

			var result = recognizer.GetRecognizerResult(entities);

			Assert.Null(result);
		}

		[Fact]
		public void DoesntMatchWhenMissingDaterange()
		{
			IRecognizer recognizer = new ReportVenditePeriodoAzienda();

			var entities = new Entity[] {
				new Entity() {Category = "Skill", Text="Vendite"},
				new Entity() {Category="Organization", Text="Splendox spa" },
			};

			var result = recognizer.GetRecognizerResult(entities);

			Assert.Null(result);
		}


		[Fact]
		public void DoesntMatchOnOtherSkill()
		{
			IRecognizer recognizer = new ReportVenditePeriodoAzienda();

			var entities = new Entity[] {
				new Entity() {Category = "Skill", Text="Acquisti"},
				new Entity() {Category = "Organization", Text="Splendox spa" },
				new Entity() {Category = "DateTime", SubCategory = "DateRange", Text= "dal 21/06/23 al 1/1/2023"}
			};

			var result = recognizer.GetRecognizerResult(entities);
			Assert.Null(result);
		}

	}
}
