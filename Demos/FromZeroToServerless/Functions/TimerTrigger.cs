using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace Functions
{
  public static class TimerTrigger
  {
    [FunctionName("TimerTrigger")]
    public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
    {
      log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
    }
  }
}
