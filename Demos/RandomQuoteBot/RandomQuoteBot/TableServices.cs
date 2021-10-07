using Azure.Data.Tables;
using System;
using System.Linq;

namespace RandomQuoteBot
{

	public class TableServices : ITableServices
	{

		private readonly string _accountName;
		private readonly string _accountKey;
		private readonly string _channelTableName;
		private readonly string _quoteTableName;

		public TableServices(
			string accountName,
			string accountKey,
			string channelTableName,
			string quoteTableName)
		{
			_accountKey = accountKey;
			_accountName = accountName;
			_channelTableName = channelTableName;
			_quoteTableName = quoteTableName;
		}

		public int AddChannel(Channel channel)
		{
			channel.LastRandomMessage = DateTime.UtcNow;
			return GetTableClient(_channelTableName).UpsertEntity(channel.ToTableRow()).Status;
		}

		public int AddQuote(Quote quote)
		{
			return GetTableClient(_quoteTableName).UpsertEntity(quote.ToTableRow()).Status;
		}

		public Quote GetQuote(
			string channelName,
			int quoteId)
		{
			return GetTableClient(_quoteTableName)
				.Query<QuoteTableRow>(s => s.PartitionKey == channelName && s.RowKey == quoteId.ToString())
				.FirstOrDefault()
				.ToEntity();
		}

		public Quote GetQuote(
			string channelName,
			int idStartRange,
			int idEndRange)
		{
			return GetQuote(channelName, new Random().Next(idStartRange, idEndRange));
		}

		private TableClient GetTableClient(string tableName)
		{
			return new TableClient(
				new Uri($"https://{_accountName}.table.core.windows.net"),
				tableName,
				new TableSharedKeyCredential(_accountName, _accountKey));
		}

	}

}