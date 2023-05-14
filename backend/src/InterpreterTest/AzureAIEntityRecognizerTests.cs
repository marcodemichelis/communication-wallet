using Interpreter.EntityRecognizers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterTest
{
	public class AzureAIEntityRecognizerTests
	{

		public AzureAIEntityRecognizerTests() { }

		[Fact]
		public async Task te()
		{
			IEntityRecognizer ai = new AzureAIEntityRecognizer();
			//string q = "Vorrei il report delle vendite del cliente Giampetruzzi Flavio dal 1/1/2023 al 30/06/2023";
			string q = "mi dai il report delle vendite di Splendox spa dal 01/01/2023 al 01/06/2023";
			//string q = "Vorrei il report delle vendite della ditta Spendox spa dal 1/1/2023 al 30/06/2023";
			//string q = "Spendox";
			//string q = "Giampetruzzi Flavio";
			//string q = "Vendite";
			//string q = "report";
			//string q = "Vorrei le vendite del cliente Giampetruzzi Flavio dal 1/1/2023 al 30/06/2023";
			//string q = "Vorrei la lista delle anagrafiche";
			//string q = "Vorrei il report delle vendite del mese da Gennaio 2023 a Aprile 2023";
			var r = await ai.GetEntitiesAsync(q);
			Assert.NotNull(r);
			string json = Newtonsoft.Json.JsonConvert.SerializeObject(r);
			
			Console.WriteLine(json);

		}


	}
}
