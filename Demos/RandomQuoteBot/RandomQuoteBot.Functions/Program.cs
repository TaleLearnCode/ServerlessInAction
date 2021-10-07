using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RandomQuoteBot
{
	public class Program
	{
		public static void Main()
		{

			JsonSerializerOptions jsonSerializerOptions = new()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
				DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
			};

			var host = new HostBuilder()
					.ConfigureFunctionsWorkerDefaults()
					.ConfigureServices(s =>
					{
						s.AddSingleton((s) => { return jsonSerializerOptions; });
						s.AddSingleton<ITwitchBot>((s) => new TwitchBot(Environment.GetEnvironmentVariable("TwitchChannelName"), Environment.GetEnvironmentVariable("TwitchAccessToken")));
						s.AddSingleton<ITableServices>((s) => new TableServices(Environment.GetEnvironmentVariable("StorageAccountName"), Environment.GetEnvironmentVariable("StorageAccountKey"), Environment.GetEnvironmentVariable("ChannelTableName"), Environment.GetEnvironmentVariable("QuoteTableName"), Environment.GetEnvironmentVariable("StorageConnectionString"), Environment.GetEnvironmentVariable("QuoteIdContainerName"), jsonSerializerOptions));
					})
					.Build();

			host.Run();
		}
	}
}