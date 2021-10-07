namespace RandomQuoteBot
{

	/// <summary>
	/// Represents a quote that the system will randomly send to subscribed Twich channels.
	/// </summary>
	public class Quote
	{

		/// <summary>
		/// Gets or sets the quote identifier.
		/// </summary>
		/// <value>A <c>string</c> representing the identiifer of the quote.</value>
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the channel the quote will be served to.
		/// </summary>
		/// <value>A <c>string</c> representing the name of the channel the quote is served to.</value>
		public string Channel { get; set; }

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