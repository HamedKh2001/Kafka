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
		private int OutputRate;
		private int MaxRange { get; }
		private int SentMessagesPerPublish;
		private int SumOfPublishedMessages;
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
			OutputRate = Convert.ToInt32(_configuration.GetSection("ProducerConfig")["OutputRate"]);
			IntervalTime = Convert.ToInt32(_configuration.GetSection("ProducerConfig")["ReportInterval"]);

			_capPublisher = capPublisher;
			Logger = logger;
			SumOfPublishedMessages = 0;


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
		public async Task<int> Publisherasync()
		{
			//Statics
			MessageFactory factory = new(MaxRange);
			var messageList = factory.GenerateMessage();
			int delayInMilliSec = 20;
			int numberOfStepsinEachSecond = 1000 / delayInMilliSec;
			int sizeOfmessageListToSend = OutputRate / numberOfStepsinEachSecond;

			while (messageList.Count >= 0)
			{
				var messagesToSend = messageList.Take(numberOfStepsinEachSecond);
				messageList.RemoveRange(0, numberOfStepsinEachSecond);
				await _capPublisher.PublishAsync(nameof(ApiMessage), contentObj: messagesToSend);
				SentMessagesPerPublish = messagesToSend.Count();
				SumOfPublishedMessages += messagesToSend.Count();
				Logger.LogWarning(CreateStatictics());
				await Task.Delay(delayInMilliSec);
			}
			return SumOfPublishedMessages;
		}
		#endregion

		#region Helper Methods
		private string CreateStatictics()
		{
			var successfullySent = (double)SentMessagesPerPublish / SumOfPublishedMessages * 100;
			return ($"{SumOfPublishedMessages}\t{successfullySent}");
		}
		private void ResetStatistics()
		{
			SentMessagesPerPublish = 0;
		}
		#endregion
	}
}
