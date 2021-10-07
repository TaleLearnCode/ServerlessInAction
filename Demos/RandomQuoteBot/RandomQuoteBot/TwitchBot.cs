using System.Diagnostics;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace RandomQuoteBot
{

	/// <summary>
	/// Provides the Twitch services for the Random Quote Bot.
	/// </summary>
	/// <seealso cref="ITwitchBot"/>
	public class TwitchBot : ITwitchBot
	{

		private readonly TwitchClient _twitchClient = new();
		private readonly string _twitchChannelName;
		private readonly string _TwichAccessToken;

		/// <summary>
		/// Initializes the TwitchBot type.
		/// </summary>
		/// <param name="twitchChannelName">Name of the channel authenticating to Twitch.</param>
		/// <param name="twitchAccessToken">The token to use to access Twitch.</param>
		public TwitchBot(
			string twitchChannelName,
			string twitchAccessToken)
		{
			_twitchChannelName = twitchChannelName;
			_TwichAccessToken = twitchAccessToken;
			_twitchClient.OnMessageSent += TwitchCLient_OnMessageSent;
		}

		/// <summary>
		/// Sends the specified message to the specified Twitch channel.
		/// </summary>
		/// <param name="destinationChannel">The Twitch channel to send the <paramref name="message"/> to.</param>
		/// <param name="message">The message to be sent to the <paramref name="destinationChannel"/>.</param>
		public void SendMessage(
			string destinationChannel,
			string message)
		{
			ConnectToDestinationChannel(destinationChannel);
			bool sendMessage = true;
			Stopwatch stopwatch = Stopwatch.StartNew();
			while (!_twitchClient.IsConnected && sendMessage)
				if (stopwatch.ElapsedMilliseconds >= 5000) sendMessage = false;
			if (sendMessage) _twitchClient.SendMessage(destinationChannel, message);
		}

		/// <summary>
		/// Connects to the destination Twitch channel.
		/// </summary>
		/// <param name="destinationChannel">Name of the Twitch channel to connect to.</param>
		private void ConnectToDestinationChannel(string destinationChannel)
		{
			_twitchClient.Initialize(new(_twitchChannelName, _TwichAccessToken), destinationChannel);
			_twitchClient.Connect();
		}

		/// <summary>
		/// Handler for the OnMessageSent event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TwitchCLient_OnMessageSent(object sender, OnMessageSentArgs e)
		{
			if (_twitchClient.IsConnected) _twitchClient.Disconnect();
		}

	}

}