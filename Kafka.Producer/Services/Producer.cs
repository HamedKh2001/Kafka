using Domain;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Nlog.Logging;
using Producer.Application.IServices;

namespace Kafka.Producer.Services
{
	public class Producer : IProducer
	{
		#region Properties
		private int CountPerStep;
		private int MaxRange { get; }
		private int SentMessagesFromStart;
		private int SentMessagesPerPublish;
		private int IntervalTime;
		private readonly ICapPublisher _capPublisher;

		protected ILogger<Producer> Logger { get; }

		System.Timers.Timer TimerReportStatistics;

		#endregion

		#region Dependency Injection
		private readonly IConfiguration _configuration;
		#endregion

		#region Ctor
		public Producer(IConfiguration configuration, ICapPublisher capPublisher, ILogger<Producer> logger)
		{
			_configuration = configuration;

			//Set Configs
			MaxRange = Convert.ToInt32(_configuration.GetSection("ProducerConfig")["MaxRange"]);
			CountPerStep = Convert.ToInt32(_configuration.GetSection("ProducerConfig")["CountPerStep"]);
			IntervalTime = Convert.ToInt32(_configuration.GetSection("ProducerConfig")["ReportInterval"]);

			_capPublisher = capPublisher;
			Logger = logger;
			SentMessagesFromStart = 0;


			TimerReportStatistics = new System.Timers.Timer();
			TimerReportStatistics.Interval = IntervalTime;
			TimerReportStatistics.Elapsed += (a, b) =>
			{
				string report = CreateStatictics();
				ResetStatistics();
				Logger.LogWarning(report);
			};
			TimerReportStatistics.Start();
		}
		#endregion

		#region IJob
		public async Task Publisherasync()
		{
			//Statics
			MessageFactory factory = new(MaxRange);
			var message = factory.GenerateMessage();
			int delayInMilliSec = 20;
			int steps = message.Count / CountPerStep;

			for (int i = 0; i < steps; i++)
			{
				var header = new Dictionary<string, string?>();
				header.Add("range", factory.Range.ToString());
				var publishableMessage = message.Skip(i * CountPerStep).Take(CountPerStep);
				await _capPublisher.PublishAsync(nameof(ApiMessage), contentObj: publishableMessage, headers: header);
				SentMessagesPerPublish = publishableMessage.Count();
				SentMessagesFromStart += publishableMessage.Count();
				Logger.LogWarning(CreateStatictics());
				Task.Delay(delayInMilliSec).Wait();
			}
		}
		#endregion

		#region Helper Methods
		private string CreateStatictics()
		{
			var successfullySent = (double)SentMessagesPerPublish / SentMessagesFromStart * 100;
			return ($"{SentMessagesFromStart}\t{successfullySent}");
		}
		private void ResetStatistics()
		{
			SentMessagesPerPublish = 0;
		}
		#endregion
	}
}
