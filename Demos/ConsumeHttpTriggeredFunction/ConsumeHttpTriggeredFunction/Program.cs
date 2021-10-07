using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsumeHttpTriggeredFunction
{

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

}
