#pragma warning disable CS8603 // Possible null reference return.
using Newtonsoft.Json;

namespace Interpreter.EntityRecognizers
{
    public class FakeAzureAIEntityRecognizer : IEntityRecognizer
    {
        public Task<Entity[]> GetEntitiesAsync(string text)
        {
			var fakeEntities = text switch
			{
				"Vorrei il report delle vendite della ditta Spendox spa dal 1/1/2023 al 30/06/2023" => JsonConvert.DeserializeObject<Entity[]>(/*lang=json,strict*/ @"
						[
							{""Text"":""report"",""Category"":""Skill"",""SubCategory"":null,""Score"":0.58,""Offset"":10},
							{""Text"":""vendite"",""Category"":""Skill"",""SubCategory"":null,""Score"":0.59,""Offset"":23},
							{""Text"":""Spendox spa"",""Category"":""Organization"",""SubCategory"":null,""Score"":0.98,""Offset"":43},
							{""Text"":""dal 1/1/2023 al 30/06/2023"",""Category"":""DateTime"",""SubCategory"":""DateRange"",""Score"":0.8,""Offset"":55}
						]
					"),

				"Vorrei il report delle vendite del cliente Giampetruzzi Flavio dal 1/1/2023 al 30/06/2023" => JsonConvert.DeserializeObject<Entity[]>(/*lang=json,strict*/ @"
						[
							{""Text"":""report"",""Category"":""Skill"",""SubCategory"":null,""Score"":0.76,""Offset"":10},
							{""Text"":""vendite"",""Category"":""Skill"",""SubCategory"":null,""Score"":0.59,""Offset"":23},
							{""Text"":""cliente"",""Category"":""PersonType"",""SubCategory"":null,""Score"":0.82,""Offset"":35},
							{""Text"":""Giampetruzzi Flavio"",""Category"":""Person"",""SubCategory"":null,""Score"":0.75,""Offset"":43},
							{""Text"":""dal 1/1/2023 al 30/06/2023"",""Category"":""DateTime"",""SubCategory"":""DateRange"",""Score"":0.8,""Offset"":63}
						]
					"),

				_ => null,
			};

			return Task.FromResult(fakeEntities ?? Array.Empty<Entity>());
		}

    }

}

#pragma warning restore CS8603 // Possible null reference return.