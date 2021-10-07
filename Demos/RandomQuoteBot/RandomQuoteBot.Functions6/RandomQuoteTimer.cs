using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace RandomQuoteBot.Functions6
{
	public class RandomQuoteTimer
	{
		private readonly ITableServices _tableServices;
		private readonly ITwitchBot _twitchBot;

		public RandomQuoteTimer(
			ITableServices tableServices,
			ITwitchBot twitchBot)
		{
			_tableServices = tableServices;
			_twitchBot = twitchBot;
		}

		[FunctionName("RandomQuoteTimer")]
		public void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
		{
			log.LogInformation($"RandomQuoteTimer started at: {DateTime.Now}");
			_tableServices.SendTimedRandomQuotes(_twitchBot, log);
			log.LogInformation($"RandomQuoteTimer finished at: {DateTime.Now}");





		}
	}
}