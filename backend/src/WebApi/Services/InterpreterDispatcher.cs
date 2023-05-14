using Interpreter;
using Interpreter.Recognizers.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Commands;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
	public class InterpreterDispatcher : IInterpreterDispatcher
	{
		private readonly IMediator _mediator;

		public InterpreterDispatcher(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<DispatchResult> DispatchAsync(string connectionId, string username, RecognizerResult result)
		{
			switch (result.OperationName)
			{
				case "ReportVendite":
					return await DispatchReportVenditeAsync(connectionId, username, result);
				default:
					return DispatchResult.Failed(ScusaRandom());
			}
		}

		private string ScusaRandom()
		{
			string[] scuseBenCongeniate = {
				"Scusa, non sono riuscito a capirti. Riprova a formulare diversamente la richiesta.",
				"Non ho capito, ma sto imparando. Puoi ripetere?",
				"Sono una macchina e per me è complicato capire le persone... ma sto imparando dagli errori.",
				"Scusa, ma i miei neuroni sono pochi e mal organizzati... potresti riperete per favore?"
			};
			var random = new Random();
			string scusa = scuseBenCongeniate[random.NextInt64(0, scuseBenCongeniate.Length)];
			return scusa;
		}

		

		private async Task<DispatchResult> DispatchReportVenditeAsync(string connectionId, string username, RecognizerResult result)
		{
			var request = new EnqueueLongReportRequest(connectionId, username, result.GetValue<string>("NomeCliente"));
			await _mediator.Send(request);
			return DispatchResult.Succes(
				$"Ok, perfetto! La richiesta per il report delle vendite " +
				$"dal {result.GetValue<DateTime>("DateStart"):dd/MM/yyyy} al {result.GetValue<DateTime>("DateEnd"):dd/MM/yyyy} " +
				$"di '{result.GetValue<string>("NomeCliente")}' è stata accodata.");
		}
		
	}
}
