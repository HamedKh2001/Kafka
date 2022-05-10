using Confluent.Kafka;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Text.Json;

namespace Kafka.Consumer.Services
{
	public class ApacheKafkaConsumerService : IHostedService
	{
		#region Properties
		private readonly string Topic;
		private readonly string GroupId;
		private readonly string BootstrapServers;
		#endregion
		#region DI
		private readonly IConfiguration _configuration;
		#endregion

		#region Ctor
		public ApacheKafkaConsumerService(IConfiguration configuration)
		{
			_configuration = configuration;
			Topic = _configuration.GetSection("KafkaConfigs").GetSection("topic").Value;
			GroupId = _configuration.GetSection("KafkaConfigs").GetSection("groupId").Value;
			BootstrapServers = _configuration.GetSection("KafkaConfigs").GetSection("bootstrapServer").Value;
		}
		#endregion


		#region Helper Methods

		#endregion


		#region IHostedService
		public Task StartAsync(CancellationToken cancellationToken)
		{
			var config = new ConsumerConfig
			{
				GroupId = GroupId,
				BootstrapServers = BootstrapServers,
				AutoOffsetReset = AutoOffsetReset.Earliest
			};

			try
			{
				using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
				{
					consumerBuilder.Subscribe("");
					var cancelToken = new CancellationTokenSource();
					try
					{
						while (true)
						{
							var consumer = consumerBuilder.Consume(cancelToken.Token);
							var orderRequestist = JsonSerializer.Deserialize<List<ApiMessage>>(consumer.Message.Value);
							Debug.WriteLine($"Processing Order Id: { orderRequestist.Count}");
						}
					}
					catch (OperationCanceledException)
					{
						consumerBuilder.Close();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			return Task.CompletedTask;
		}
		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
		#endregion

	}
}
