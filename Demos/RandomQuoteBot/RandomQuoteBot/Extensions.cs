using System.Collections.Generic;
using System.Linq;

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

		internal static Channel ToEntity(this ChannelTableRow channelTableRow)
		{
			return new()
			{
				ChannelName = channelTableRow.RowKey,
				MessageFrequency = channelTableRow.MessageFrequency,
				LastRandomMessage = channelTableRow.LastRandomMessage
			};
		}

		internal static List<Channel> ToEntityList(this List<ChannelTableRow> channelTableRows)
		{
			List<Channel> channels = new();
			if (channelTableRows != null && channelTableRows.Any())
				foreach (ChannelTableRow row in channelTableRows)
					channels.Add(row.ToEntity());
			return channels;
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

		internal static Quote ToEntity(this QuoteTableRow quoteTableRow)
		{
			return new()
			{
				Channel = quoteTableRow.PartitionKey,
				Id = quoteTableRow.RowKey,
				Author = quoteTableRow.Author,
				Text = quoteTableRow.Text
			};
		}

	}
}