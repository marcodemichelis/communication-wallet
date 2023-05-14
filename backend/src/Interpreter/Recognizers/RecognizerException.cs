using System.Runtime.Serialization;

namespace Interpreter.Recognizers
{
	[Serializable]
	public class RecognizerException : Exception
	{
		public RecognizerException()
		{
		}

		public RecognizerException(string? message) : base(message)
		{
		}

		public RecognizerException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

		protected RecognizerException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}