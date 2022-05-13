using Consomer.Application.IService;
using Domain;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Kafka.Consumer.Services
{
	public class SubscriberService : ISubscriberService, ICapSubscribe
	{
		#region Properties
		private ConcurrentDictionary<int, bool> RecievedMessage;
		private string? TempIdentifier;
		private int Delay;
		#endregion

		#region DI
		public readonly IConfiguration _configuration;
		public readonly ILogger _logger;
		#endregion

		#region Ctor
		public SubscriberService(IConfiguration configuration,ILogger<SubscriberService> logger)
		{
			_configuration = configuration;
			RecievedMessage = new ConcurrentDictionary<int, bool>();
			Delay = Convert.ToInt32(_configuration.GetSection("SubscribeConfig")["Delay"]);
			_logger = logger;
		}
		#endregion


		[CapSubscribe(nameof(ApiMessage))]
		public Task CapListener(List<ApiMessage> messages, [FromCap] CapHeader header)
		{
			using (var cts = new CancellationTokenSource(1000))
			{
				//Task.Factory.StartNew(async () =>await CheckReceivedMessage(messages, header), cts.Token);
				Task.Run(async () => await CheckReceivedMessageasync(messages, header), cts.Token);
			}
			var successRate = (double)RecievedMessage.Count() / Convert.ToDouble(header.GetValueOrDefault("range")) * 100;
			Debug.WriteLine($"success rate = {successRate}%");
			_logger.LogInformation($"success rate = {successRate}%");
			return Task.CompletedTask;
		}

		#region ISubscriberService
		public Task CheckReceivedMessageasync(List<ApiMessage> messages, [FromCap] CapHeader header)
		{
			if (header.GetValueOrDefault("identifier") != TempIdentifier && TempIdentifier != null)
				RecievedMessage = new ConcurrentDictionary<int, bool>();
			TempIdentifier = header.GetValueOrDefault("identifier");
			var range = Convert.ToInt32(header.GetValueOrDefault("range"));
			Parallel.ForEach(messages, message =>
			{
				new ParallelOptions
				{
					MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 2.0))
				};
				RecievedMessage.TryAdd(message.Id, true);
				//for (int i = RecievedMessage.Count + 1; i <= range; i++)
				//{
				//RecievedMessage.TryAdd(i, true);
				//if (messages.Any(m => m.Id == i))
				//	RecievedMessage.TryAdd(i, true);
				//else if (RecievedMessage.Any(r => r.Key == i & r.Value == false))
				//	RecievedMessage.TryAdd(i, false);
				//}
			});
			return Task.CompletedTask;
		}
		#endregion
	}
}
