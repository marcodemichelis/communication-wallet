namespace WebApi.Services.Interfaces
{
	public class DispatchResult
	{
		public static DispatchResult Succes(string message) {
			return new DispatchResult()
			{
				Message = message,
				Success = true
			};
		}
		public static DispatchResult Failed(string message)
		{
			return new DispatchResult()
			{
				Message = message,
				Success = false
			};
		}


		public string Message { get; set; } = "";
		public bool Success { get; set; } = false;
	}
}
