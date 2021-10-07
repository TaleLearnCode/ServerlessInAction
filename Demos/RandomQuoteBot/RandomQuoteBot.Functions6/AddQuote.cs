using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RandomQuoteBot
{
	public class AddQuote
	{

		private readonly ITableServices _tableServices;
		private readonly JsonSerializerOptions _jsonSerializerOptions;

		public AddQuote(
			ITableServices tableServices,
			JsonSerializerOptions jsonSerializerOptions)
		{
			_tableServices = tableServices;
			_jsonSerializerOptions = jsonSerializerOptions;
		}

		[FunctionName("AddQuote")]
		public async Task<IActionResult> AddQuoteAsync(
			[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "channels/{channelName}/quotes")] HttpRequest request,
			[Blob("quoteids/{channelName}", FileAccess.Read, Connection = "StorageConnectionString")] Stream readQuoteId,
			[Blob("quoteids/{channelName}", FileAccess.Write, Connection = "StorageConnectionString")] Stream writeQuoteId,
			ILogger log,
			string channelName)
		{

			log.LogInformation("C# HTTP trigger function processed a request.");

			try
			{

				QuoteId quoteId = await DeserializeQuoteIdAsync(readQuoteId, channelName);

				Quote quote = JsonSerializer.Deserialize<Quote>(await new StreamReader(request.Body).ReadToEndAsync(), _jsonSerializerOptions);
				quote.Id = quoteId.MaxId.ToString();
				int addQuoteResult = _tableServices.AddQuote(quote);

				if (addQuoteResult == 204)
					writeQuoteId.Write(Encoding.ASCII.GetBytes(JsonSerializer.Serialize(quoteId, _jsonSerializerOptions)));

				return new StatusCodeResult(addQuoteResult);

			}
			catch (Exception ex)
			{
				log.LogError($"AddChannel: {ex.Message}");
				return new StatusCodeResult(500);
			}

		}

		private async Task<QuoteId> DeserializeQuoteIdAsync(Stream quoteId, string channelName)
		{

			QuoteId results;
			try
			{
				string requestBody = await new StreamReader(quoteId).ReadToEndAsync();
				results = JsonSerializer.Deserialize<QuoteId>(requestBody, _jsonSerializerOptions);
			}
			catch
			{
				results = null;
			}

			if (results == null) results = new(channelName);
			results.MaxId = ++results.MaxId;

			return results;

		}

	}

}
