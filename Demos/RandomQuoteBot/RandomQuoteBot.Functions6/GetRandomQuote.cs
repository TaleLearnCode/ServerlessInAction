using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace RandomQuoteBot.Functions6
{
	public class GetRandomQuote
	{

		private readonly ITableServices _tableServices;
		private readonly JsonSerializerOptions _jsonSerializerOptions;

		public GetRandomQuote(
			ITableServices tableServices,
			JsonSerializerOptions jsonSerializerOptions)
		{
			_tableServices = tableServices;
			_jsonSerializerOptions = jsonSerializerOptions;
		}

		[FunctionName("GetRandomQuote")]
		public async Task<IActionResult> RunAsync(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "channels/{channelName}/quotes")] HttpRequest request,
			[Blob("quoteids/{channelName}", FileAccess.Read, Connection = "StorageConnectionString")] Stream readQuoteId,
			ILogger log,
			string channelName)
		{
			log.LogInformation($"GetRandomQuote: {channelName}");
			try
			{
				QuoteId quoteId = JsonSerializer.Deserialize<QuoteId>(await new StreamReader(readQuoteId).ReadToEndAsync(), _jsonSerializerOptions);
				Quote quote = _tableServices.GetQuote(channelName, 1, quoteId.MaxId);
				if (quote != null)
					return new OkObjectResult(quote);
				else
					return new NotFoundResult();
			}
			catch (Exception ex)
			{
				log.LogError($"GetRandomQuote: {ex.Message}");
				return new StatusCodeResult(500);
			}
		}

	}

}