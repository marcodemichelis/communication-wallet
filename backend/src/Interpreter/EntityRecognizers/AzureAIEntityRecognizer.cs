using Azure;
using Azure.AI.TextAnalytics;

namespace Interpreter.EntityRecognizers
{
	public class AzureAIEntityRecognizer : IEntityRecognizer
	{
		private const string LANGUAGE_KEY = "TBD";
		private const string LANGUAGE_ENDPOINT = "https://sentimientos.cognitiveservices.azure.com/";
		private readonly TextAnalyticsClient textAnalyticsClient;

		public AzureAIEntityRecognizer()
		{
			var credentials = new AzureKeyCredential(LANGUAGE_KEY);
			var endpoint = new Uri(LANGUAGE_ENDPOINT);
			var options = new TextAnalyticsClientOptions()
			{
				DefaultLanguage = "it",
				DefaultCountryHint = "it"		
			};

			textAnalyticsClient = new TextAnalyticsClient(endpoint, credentials, options);
		}



		public async Task<Entity[]> GetEntitiesAsync(string text)
		{
			var entities = new List<Entity>();
			var azureEntities = await textAnalyticsClient.RecognizeEntitiesAsync(text);
			foreach (var e in azureEntities.Value)
			{
				entities.Add(new Entity()
				{
					Text = e.Text,
					Category = e.Category.ToString(),
					SubCategory = e.SubCategory,
					Score = e.ConfidenceScore,
					Offset = e.Offset,
				});
			}

			return entities.ToArray();
		}

	}

}
