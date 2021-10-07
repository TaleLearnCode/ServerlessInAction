namespace RandomQuoteBot
{
	public interface ITableServices
	{
		int AddChannel(Channel channel);
		int AddQuote(Quote quote);
		Quote GetQuote(string channelName, int quoteId);
	}
}