using Azure;
using Azure.Data.Tables;
using System;

namespace RandomQuoteBot
{

	internal class QuoteTableRow : ITableEntity
	{
		public string PartitionKey { get; set; }
		public string RowKey { get; set; }
		public DateTimeOffset? Timestamp { get; set; }
		public ETag ETag { get; set; }

		public string Author { get; set; }

		public string Text { get; set; }

	}

}