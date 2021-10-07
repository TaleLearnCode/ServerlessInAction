namespace RandomQuoteBot
{

	/// <summary>
	/// Represents a quote that the system will randomly send to subscribed Twich channels.
	/// </summary>
	public class Quote
	{

		public string PartitionKey { get; set; }

		public string RowKey { get; set; }

		/// <summary>
		/// Gets or sets the text of the quote.
		/// </summary>
		/// <value>A <c>string</c> representing the text of the quote.</value>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the quote's author.
		/// </summary>
		/// <value>A <c>string</c> representing the author of the quote.</value>
		public string Author { get; set; }

	}

}