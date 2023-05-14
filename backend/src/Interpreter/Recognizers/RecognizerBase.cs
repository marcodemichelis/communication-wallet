using Interpreter.Recognizers.Result;
using Interpreter.Recognizers.Rule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Interpreter.Recognizers
{
	public abstract class RecognizerBase
	{

		protected Parameter[] GetFullTextParameter(Entity e, string name) =>
			new Parameter[] { new Parameter(name, e.Text) };

		protected Parameter[] GetNomeClienteParameter(Entity e) => GetFullTextParameter(e, "NomeCliente");



		protected Parameter[] GetDateStartDateEndParameters(Entity e)
		{
			string datePattern = @"\d{1,2}\W\d{1,2}\W\d{2,4}";
			var matches = Regex.Matches(e.Text, datePattern);
			if (matches.Count == 2)
			{
				var start = ParseDateOrFails(matches[0].Value);
				var end = ParseDateOrFails(matches[1].Value);
				if (start > end) (end, start) = (start, end);
				return new Parameter[] { new Parameter("DateStart", start), new Parameter("DateEnd", end) };
			}
			else if (matches.Count == 1)
			{
				var start = ParseDateOrFails(matches[0].Value);
				var end = DateTime.Now.Date;
				if (start > end) (end, start) = (start, end);
				return new Parameter[] { new Parameter("DateStart", start), new Parameter("DateEnd", end) };
			}
			return Array.Empty<Parameter>();
		}


		private static CultureInfo italian = new CultureInfo("it-IT");
		protected DateTime ParseDateOrFails(string text)
		{
			if (DateTime.TryParse(text, italian, out var date))
				return date;
			else
				throw new RecognizerException($"Non riesco a riconoscere {text} come una data valida.");
		}


		protected RecognizerResult? CreateResult(string operationName, IEnumerable<EntityRule> rules, IEnumerable<Entity> entities)
		{
			var result = new RecognizerResult(operationName);
			foreach (var rule in rules)
			{
				var matchEnt = entities.FirstOrDefault(rule.Match);
				if (matchEnt == null) { return null; }
				result.Parameters.AddRange(rule.ExtractParams(matchEnt));
			}
			return result;
		}

	}
}
