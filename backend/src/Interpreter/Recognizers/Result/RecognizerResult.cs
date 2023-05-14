namespace Interpreter.Recognizers.Result
{
	public class RecognizerResult
	{
		private static readonly RecognizerResult empty = new("");
		public static RecognizerResult Empty => empty;

		public RecognizerResult(string operationName)
		{
			OperationName = operationName;
		}
		public string OperationName { get; private set; }
		public List<Parameter> Parameters { get; private set; } = new List<Parameter>();

		public T GetValue<T>(string name)
		{
			var param = Parameters.FirstOrDefault(p => p.Name == name);
			
			if (param == null)
				throw new NullReferenceException($"Nessun parametro con nome '{name}' presente");
			
			if (param.Value is T)
				return (T)param.Value;
			
			throw new Exception($"Tipo errato");
		}

	}
}
