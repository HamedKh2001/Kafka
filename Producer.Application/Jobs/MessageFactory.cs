using Domain;
using Microsoft.Extensions.Configuration;

namespace Producer.Application.Jobs
{
	public class MessageFactory
	{
		#region Properties
		public int Range { get; }
		#endregion
		
		#region Ctor
		public MessageFactory(int range)
		{
			this.Range = range;
		}
		#endregion

		public List<ApiMessage> GenerateMessage()
		{
			int counter = 1;
			List<ApiMessage> messageList = new();
			var rnd = new Random();
			while (counter <= this.Range)
			{
				messageList.Add(new ApiMessage
				{
					Id = counter,
					DateTime = DateTime.Now,
					Value = rnd.Next(1,100000000),
				});
				counter++;
			}
			return messageList;
		}
	}
}
