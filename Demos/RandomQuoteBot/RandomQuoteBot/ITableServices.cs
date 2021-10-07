using Microsoft.Extensions.Logging;

namespace RandomQuoteBot
{
	public interface ITableServices
	{
		int AddChannel(Channel channel);
		int AddQuote(Quote quote);
		Quote GetQuote(string channelName, int quoteId);
		Quote GetQuote(string channelName, int idStartRange, int idEndRange);
		void SendTimedRandomQuotes(ITwitchBot twitchBot, ILogger log);
	}
}