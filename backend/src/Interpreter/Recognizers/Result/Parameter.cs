namespace Interpreter.Recognizers.Result
{
	public class Parameter
	{
		public Parameter(string name, object? value)
		{
			Name = name;
			Value = value;
		}

		private static Parameter _empty = new Parameter("", null);
		public static Parameter Empty { get => _empty; }

		public string Name { get; private set; }
		public object? Value { get; private set; }
		


	}
}
