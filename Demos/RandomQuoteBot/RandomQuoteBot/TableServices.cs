using Azure.Data.Tables;
using System;

namespace RandomQuoteBot
{

	public class TableServices : ITableServices
	{

		private readonly string _accountName;
		private readonly string _accountKey;
		private readonly string _channelTableName;

		public TableServices(
			string accountName,
			string accountKey,
			string channelTableName)
		{
			_accountKey = accountKey;
			_accountName = accountName;
			_channelTableName = channelTableName;
		}

		public int AddChannel(Channel channel)
		{
			channel.LastRandomMessage = DateTime.UtcNow;
			return GetTableClient(_channelTableName).UpsertEntity(channel.ToChannelTableRow()).Status;
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