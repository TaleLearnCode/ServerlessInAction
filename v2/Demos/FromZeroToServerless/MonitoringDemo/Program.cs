using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MonitoringDemo
{
  internal class Program
  {

    //HttpClient client = new HttpClient();

    private static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");

      HttpClient client = new HttpClient();

      ExecuteAsync(client);

      Console.WriteLine("Done");
      

    }

    public static async Task<string> GetAsync(HttpClient client)
    {
      HttpResponseMessage response = await client.GetAsync("https://fromzerotoserverless.azurewebsites.net/api/HelloWorld?code=bVSPG2iSTh2iWrFUK6bmMuwqtqKrmsKmu6K2U6KCdbUzVfm14jiUsQ==&name=Corina");
      if (response.IsSuccessStatusCode)
      {
        return await response.Content.ReadAsStringAsync();
      }
      return string.Empty;
    }

    public static async Task ExecuteAsync(HttpClient client)
    {
      await client.GetAsync("https://fromzerotoserverless.azurewebsites.net/api/HelloWorld?code=bVSPG2iSTh2iWrFUK6bmMuwqtqKrmsKmu6K2U6KCdbUzVfm14jiUsQ==&name=Corina");
      Console.WriteLine("HTTP Call Complete");
    }


  }
}