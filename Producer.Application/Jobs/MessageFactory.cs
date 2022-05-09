using Domain;

namespace Producer.Application.Jobs
{
	public class MessageFactory
	{
		public ApiMessage GenerateMessage()
		{
			var rnd = new Random();
			var message = new ApiMessage
			{
				Id = rnd.Next(1, 500),
				Timespan = TimeSpan.FromSeconds(60),
				Value = rnd.Next(),
			};
			return message;
		}
	}
}
