using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TaleLearnCode;

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

		[Function("AddChannel")]
		public async Task<HttpResponseData> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "channels/{channelName}")] HttpRequestData request,
			FunctionContext executionContext,
			string channelName)
		{

			ILogger logger = executionContext.GetLogger("AddChannel");
			logger.LogInformation($"AddChannel: {channelName}");

			try
			{
				Channel channel = await request.GetRequestParametersAsync<Channel>(_jsonSerializerOptions);
				channel.ChannelName = channelName;
				return request.CreateResponse((HttpStatusCode)_tableServices.AddChannel(channel));
			}
			catch (Exception ex)
			{
				return request.CreateErrorResponse(ex);
			}

		}
	}
}