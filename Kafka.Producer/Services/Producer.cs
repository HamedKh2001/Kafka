using Domain;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Producer.Application.IServices;

namespace Kafka.Producer.Services
{
	public class Producer : IProducer
	{
		#region Properties
		private int CountPerStep;
		private int MaxRange { get; }
		private readonly ICapPublisher _capPublisher;
		#endregion

		#region Dependency Injection
		private readonly IConfiguration _configuration;
		#endregion

		#region Ctor
		public Producer(IConfiguration configuration, ICapPublisher capPublisher)
		{
			_configuration = configuration;
			MaxRange = Convert.ToInt32(_configuration.GetSection("ProducerConfig")["MaxRange"]);
			CountPerStep = Convert.ToInt32(_configuration.GetSection("ProducerConfig")["CountPerStep"]);
			_capPublisher=capPublisher;
		}
		#endregion

		#region IJob
		public async Task Publisherasync()
		{
			MessageFactory factory = new(MaxRange);
			var message = factory.GenerateMessage();
			for (int i = 0; i < message.Count / CountPerStep; i++)
			{
				var header = new Dictionary<string, string?>();
				header.Add("range", factory.Range.ToString());
				var publishableMessage = message.Skip(i * CountPerStep).Take(CountPerStep);
				await _capPublisher.PublishAsync(nameof(ApiMessage), contentObj: publishableMessage, headers: header);
				Task.Delay(800).Wait();
			}
		}
		#endregion
	}
}
