using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RandomQuoteBot
{

	public class TableServices : ITableServices
	{

		private readonly string _accountName;
		private readonly string _accountKey;
		private readonly string _channelTableName;
		private readonly string _quoteTableName;
		private readonly string _storageConnectionString;
		private readonly string _quoteIdContainerName;
		private readonly JsonSerializerOptions _jsonSerializerOptions;

		public TableServices(
			string accountName,
			string accountKey,
			string channelTableName,
			string quoteTableName,
			string storageConnectionString,
			string quoteIdContainerName,
			JsonSerializerOptions jsonSerializerOptions)
		{
			_accountKey = accountKey;
			_accountName = accountName;
			_channelTableName = channelTableName;
			_quoteTableName = quoteTableName;
			_storageConnectionString = storageConnectionString;
			_quoteIdContainerName = quoteIdContainerName;
			_jsonSerializerOptions = jsonSerializerOptions;
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

		public List<Channel> GetChannels()
		{
			return GetTableClient(_channelTableName)
				.Query<ChannelTableRow>(s => s.PartitionKey == "RandomQuoteBot")
				.ToList()
				.ToEntityList();
		}

		public async void SendTimedRandomQuotes(
			ITwitchBot twitchBot,
			ILogger log)
		{
			List<Channel> channels = GetTableClient(_channelTableName)
				.Query<ChannelTableRow>(s => s.PartitionKey == "RandomQuoteBot")
				.ToList()
				.ToEntityList();

			foreach (Channel channel in channels)
			{
				if (DateTime.UtcNow >= channel.LastRandomMessage.AddMinutes(channel.MessageFrequency))
				{
					Quote quote = GetQuote(channel.ChannelName, 1, (await GetMaxQuoteId(channel.ChannelName)).MaxId);
					twitchBot.SendQuoteMessage(channel.ChannelName, quote);
					channel.LastRandomMessage = DateTime.UtcNow;
					GetTableClient(_channelTableName).UpsertEntity(channel.ToTableRow());
					log.LogInformation($"RandomQuoteTimer: {channel.ChannelName}|{quote.Text} - {quote.Author}");
				}
			}

		}

		private async Task<QuoteId> GetMaxQuoteId(string channelName)
		{

			BlobContainerClient container = new(_storageConnectionString, _quoteIdContainerName);
			BlobClient blob = container.GetBlobClient(channelName);
			BlobDownloadInfo download = blob.Download();
			QuoteId quoteId;
			using (MemoryStream memoryStream = new())
			{
				download.Content.CopyTo(memoryStream);
				memoryStream.Position = 0;
				quoteId = JsonSerializer.Deserialize<QuoteId>(await new StreamReader(memoryStream).ReadToEndAsync(), _jsonSerializerOptions);
			}
			return quoteId;
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