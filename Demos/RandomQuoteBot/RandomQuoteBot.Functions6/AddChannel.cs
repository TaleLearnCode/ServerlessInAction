using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RandomQuoteBot
{
	public class AddChannel
	{

		private readonly ITableServices _tableServices;
		private readonly JsonSerializerOptions _jsonSerializerOptions;

		public AddChannel(
			ITableServices tableServices,
			JsonSerializerOptions jsonSerializerOptions)
		{
			_tableServices = tableServices;
			_jsonSerializerOptions = jsonSerializerOptions;
		}

		[FunctionName("AddChannel")]
		public async Task<IActionResult> AddChannelAsync(
			[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "channels/{channelName}")] HttpRequest request,
			ILogger log,
			string channelName)
		{
			log.LogInformation($"AddChannel: {channelName}");

			try
			{
				Channel channel = JsonSerializer.Deserialize<Channel>(await new StreamReader(request.Body).ReadToEndAsync(), _jsonSerializerOptions);
				channel.ChannelName = channelName;
				return new StatusCodeResult(_tableServices.AddChannel(channel));
			}
			catch (Exception ex)
			{
				log.LogError($"AddChannel: {ex.Message}");
				return new StatusCodeResult(500);
			}

		}
	}
}