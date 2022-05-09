using Domain;
using DotNetCore.CAP;
using Producer.Application.IServices;

namespace Producer.Application.Services
{
	public class Producer : IProducer
	{
		private readonly string topic = "tutorial";
		private readonly ICapPublisher _capPublisher;

		public Producer(ICapPublisher capPublisher)
		{
			_capPublisher = capPublisher;
		}

		public async Task<bool> Publish(ApiMessage apiMessage)
		{
			try
			{
				var header = new Dictionary<string, string>();
				header.Add("myKeyheader", "myheader");
				await _capPublisher.PublishAsync(topic, contentObj: apiMessage, header);
				return await Task.FromResult(true);
			}
			catch (Exception ex)
			{
				return await Task.FromResult(false);
			}
		}
	}
}
