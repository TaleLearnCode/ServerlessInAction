namespace RandomQuoteBot
{
	internal static class Extensions
	{

		internal static ChannelTableRow ToTableRow(this Channel channel)
		{
			return new()
			{
				PartitionKey = "RandomQuoteBot",
				RowKey = channel.ChannelName,
				MessageFrequency = channel.MessageFrequency,
				LastRandomMessage = channel.LastRandomMessage
			};
		}

		internal static QuoteTableRow ToTableRow(this Quote quote)
		{
			return new()
			{
				PartitionKey = quote.Channel,
				RowKey = quote.Id,
				Author = quote.Author,
				Text = quote.Text
			};
		}

	}
}