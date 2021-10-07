# Serverless in Action Demos

1. [Create an Azure Function App in the Portal](#create-an-azure-function-app-in-the-portal)
2. [Azure Functions Portal Features](#azure-functions-portal-features)
3. [Call Azure Function from C# App](#call-azure-function-from-c-app)
4. [Setup Random Quote Solution](#setup-random-quote-solution)

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
* Paste the following files from the source demo project: Channel, ITwitchBot, Quote, TwitchBot

### Random Quote Bot - Add Channel

* Add an Azure Functions project to the RandomQuoteBot solution named 'RandomQuoteBot.Functions'
* Include a .NET 5 Http triggered function
* Delete Function1.cs
* Add a project reference to RandomQuoteBot
* Add the following settings to local.settings.json: TwitchChannelName, TwitchAcessToken, StorageConnectionString
* Add the following to the Main method:

~~~
using Microsoft.Extensions.DepenencyInjection;
using System;

.ConfigureServices(s =>
{
	s.AddSingleton<ITwitchBot>((s) => new TwitchBot(Environment.GetEnvironmentVariable("TwitchChannelName"), Environment.GetEnvironmentVariable("TwitchAccessToken")));
})
~~~