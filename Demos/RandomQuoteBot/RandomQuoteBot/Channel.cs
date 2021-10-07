using System;

namespace RandomQuoteBot
{

	/// <summary>
	/// Represents a subscribed Twitch channel.
	/// </summary>
	public class Channel
	{

		/// <summary>
		/// Gets or sets the name of the subscribed Twitch channel.
		/// </summary>
		/// <value>A <c>string</c> representing the name of the subscribed Twitch channel.</value>
		public string ChannelName { get; set; }

		/// <summary>
		/// Gets or sets the frequency quote messages should be sent.
		/// </summary>
		/// <value>An <c>int</c> representing the number of minutes between quote messages.</value>
		public int MessageFrequency { get; set; }

		/// <summary>
		/// Gets or sets the date and time the last random message was sent.
		/// </summary>
		/// <value>A <c>DateTime</c> representing the date and time the last random message was sent.</value>
		public DateTime LastRandomMessage { get; set; }

	}

}