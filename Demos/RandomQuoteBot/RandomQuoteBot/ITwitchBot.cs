﻿namespace RandomQuoteBot
{
	public interface ITwitchBot
	{
		void SendMessage(string destinationChannel, string message);
	}
}