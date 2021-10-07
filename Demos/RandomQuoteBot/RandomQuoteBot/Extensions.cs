namespace RandomQuoteBot
{
	internal static class Extensions
	{

		internal static ChannelTableRow ToChannelTableRow(this Channel channel)
		{
			return new()
			{
				PartitionKey = "RandomQuoteBot",
				RowKey = channel.ChannelName,
				MessageFrequency = channel.MessageFrequency,
				LastRandomMessage = channel.LastRandomMessage
			};
		}

	}
}