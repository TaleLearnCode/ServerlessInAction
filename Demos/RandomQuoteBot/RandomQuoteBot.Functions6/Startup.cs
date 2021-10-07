using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

[assembly: FunctionsStartup(typeof(RandomQuoteBot.Startup))]

namespace RandomQuoteBot
{

	public class Startup : FunctionsStartup
	{

		public override void Configure(IFunctionsHostBuilder builder)
		{

			JsonSerializerOptions jsonSerializerOptions = new()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
				DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
			};

			builder.Services.AddSingleton((s) => { return jsonSerializerOptions; });
			builder.Services.AddSingleton<ITwitchBot>((s) => { return new TwitchBot(Environment.GetEnvironmentVariable("TwitchChannelName"), Environment.GetEnvironmentVariable("TwitchAccessToken")); });
			builder.Services.AddSingleton<ITableServices>((s) => { return new TableServices(Environment.GetEnvironmentVariable("StorageAccountName"), Environment.GetEnvironmentVariable("StorageAccountKey"), Environment.GetEnvironmentVariable("ChannelTableName"), Environment.GetEnvironmentVariable("QuoteTableName"), Environment.GetEnvironmentVariable("StorageConnectionString"), Environment.GetEnvironmentVariable("QuoteIdContainerName"), jsonSerializerOptions); });

		}

	}
}