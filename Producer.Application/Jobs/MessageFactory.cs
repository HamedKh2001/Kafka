using Domain;

namespace Producer.Application.Jobs
{
	public class MessageFactory
	{
		#region Properties
		public int range { get; }
		#endregion
		#region Ctor
		public MessageFactory(int rage)
		{
			this.range = rage;
		}
		#endregion

		public List<ApiMessage> GenerateMessage()
		{
			int counter = 1;
			List<ApiMessage> messageList = new();
			var rnd = new Random();
			while (counter <= this.range)
			{
				messageList.Add(new ApiMessage
				{
					Id = rnd.Next(1, 100),
					DateTime = DateTime.Now,
					Value = counter
				});
				counter++;
			}
			return messageList;
		}
	}
}
