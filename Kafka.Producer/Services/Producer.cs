using Domain;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Producer.Application.IServices;

namespace Kafka.Producer.Services
{
	public class Producer : IProducer
	{
		#region Properties
		public string topic => _configuration.GetSection("KafkaConfigs")["topic"];
		private readonly ICapPublisher _capPublisher;
		private readonly IConfiguration _configuration;
		#endregion

		#region Ctor
		public Producer(ICapPublisher capPublisher, IConfiguration configuration)
		{
			_capPublisher = capPublisher;
			_configuration = configuration;
		}
		#endregion

		#region IProducer
		public async Task<bool> Publish(List<ApiMessage> apiMessage, int range)
		{
			try
			{
				var header = new Dictionary<string, string>();
				header.Add("range", range.ToString());
				await _capPublisher.PublishAsync(topic, contentObj: apiMessage, headers: header);
				return await Task.FromResult(true);
			}
			catch (Exception ex)
			{
				return await Task.FromResult(false);
			}
		}
		#endregion
	}
}
