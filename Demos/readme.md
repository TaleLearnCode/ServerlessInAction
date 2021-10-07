# Serverless in Action Demos

1. [Create an Azure Function App in the Portal](#create-an-azure-function-app-in-the-portal)
2. [Azure Functions Portal Features](#azure-functions-portal-features)
3. [Call Azure Function from C# App](#call-azure-function-from-c-app)
4. [Setup Random Quote Solution](#setup-random-quote-solution)
5. [Random Quote Bot (Add Channel)](#random-quote-bot-add-channel)
---

### Create an Azure Function App in the Portal

* From the Azure Portal, add an Azure FUnction App resource talking through the wizard steps
* Go to the Functions blad and create a new HTTP Trigger
* Go to 'Code + Test'
* Talk through the generated code
* Show the Logs
* Show the Test/Run
* Execute from Postman

---

### Azure Functions Portal Features

* Talk about Configuration (mention Azure App Configuration service)
* Talk about App Keys
* Talk about Deployment Slots
* Talk about Application Insights (show Live Metrics)

---

### Call Azure Function from C# App

Create a new Console App in Visual Studio and write the following code:
~~~
	internal class Program
	{

		static readonly HttpClient httpClient = new();

		public static async Task Main()
		{
			Console.WriteLine("Press any key to start...");
			Console.ReadKey(true);
			try
			{
				HttpResponseMessage response = await httpClient.GetAsync("https://func-serverlessinaction-use.azurewebsites.net/api/HttpTrigger1?code=LGjgMY3nlNN5QoY8a1nBGdCkBq5R8ao7sy/f01Fjs8/W6vXMa/0X8g==&name=RVNUG");
				response.EnsureSuccessStatusCode();
				string responseBody = await response.Content.ReadAsStringAsync();
				Console.WriteLine(responseBody);
			}
			catch (HttpRequestException ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
				Console.ForegroundColor = ConsoleColor.White;
			}
		}

	}

~~~

---

### Setup Random Quote Solution

* Create a blank solution called RandomQuoteBot
* Create a class library project called RandomQuoteBot
* Add the TwitchLib NuGet package
* Copy the interfaces and classes from the pre-built demo project

### Random Quote Bot (Add Channel)

* Add an Azure Functions project to the RandomQuoteBot solution named 'RandomQuoteBot.Functions'
* Delete Function1.cs
* Add a project reference to RandomQuoteBot
* Add the following NuGet packages: TaleLearnCode
* Add the following settings to local.settings.json: TwitchChannelName, TwitchAcessToken, StorageConnectionString
* Update Program.cs

~~~
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
						s.AddSingleton<ITableServices>((s) => new TableServices(Environment.GetEnvironmentVariable("StorageAccountName"), Environment.GetEnvironmentVariable("StorageAccountKey"), Environment.GetEnvironmentVariable("ChannelTableName")));
					})
					.Build();

			host.Run();
		}
	}
}
~~~

* Add HTTP triggred function named AddChannel

~~~
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
~~~