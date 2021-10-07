using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace RandomQuoteBot
{

	public class GetQuote
	{

		private readonly ITableServices _tableServices;
		private readonly JsonSerializerOptions _jsonSerializerOptions;

		public GetQuote(
			ITableServices tableServices,
			JsonSerializerOptions jsonSerializerOptions)
		{
			_tableServices = tableServices;
			_jsonSerializerOptions = jsonSerializerOptions;
		}

		[FunctionName("GetQuote")]
		public IActionResult Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "channels/{channelName}/quotes/{quoteId}")] HttpRequest request,
			ILogger log,
			string channelName,
			int quoteId)
		{
			log.LogInformation($"GetQuote: {channelName}|{quoteId}");
			try
			{
				Quote quote = _tableServices.GetQuote(channelName, quoteId);
				if (quote != null)
					return new OkObjectResult(quote);
				else
					return new NotFoundResult();
			}
			catch (Exception ex)
			{
				log.LogError($"GetQuote: {ex.Message}");
				return new StatusCodeResult(500);
			}
		}

	}

}