using Consomer.Application.IService;
using Domain;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Kafka.Consumer.Services
{
	public class SubscriberService : ISubscriberService
	{
		#region Properties
		private ConcurrentDictionary<int, bool> RecievedMessage;
		private readonly int IntervalTime;
		private int InpuRate;
		private double MissRate;
		private readonly int RecieveRange;

		System.Timers.Timer TimerReportStatistics;

		System.Timers.Timer CalculateReportStatistics;

		#endregion

		#region Dependencies
		public readonly IConfiguration _configuration;
		protected ILogger<SubscriberService> Logger { get; }
		#endregion

		#region Ctor
		public SubscriberService(IConfiguration configuration, ILogger<SubscriberService> logger)
		{
			_configuration = configuration;
			RecievedMessage = new ConcurrentDictionary<int, bool>();
			IntervalTime = Convert.ToInt32(_configuration.GetSection("SubscribeConfig")["IntervalTime"]);
			RecieveRange = Convert.ToInt32(_configuration.GetSection("SubscribeConfig")["RecieveRange"]);
			InpuRate = 0;
			Logger = logger;

			InitialRecievedMessages(RecieveRange);

			TimerReportStatistics = new System.Timers.Timer();
			TimerReportStatistics.Interval = IntervalTime;
			TimerReportStatistics.Elapsed += (a, b) =>
			{
				string report = CreateStatiustics();
				ResetStatistics();
				Logger.LogWarning(report);
			};
			TimerReportStatistics.Start();



			CalculateReportStatistics = new System.Timers.Timer();
			CalculateReportStatistics.Interval = 1000;
			CalculateReportStatistics.Elapsed += (a, b) =>
			{
				CalculateMissRate();
			};
			CalculateReportStatistics.Start();
		}
		#endregion

		#region Helper Methods
		[CapSubscribe(nameof(ApiMessage))]
		private Task CapListener(List<ApiMessage> messages, [FromCap] CapHeader header)
		{
			InpuRate += messages.Count;

			Task.Run(async () => await CheckReceivedMessageasync(messages, header)).Wait();
			return Task.CompletedTask;
		}

		private Task InsertToLocalCache(List<ApiMessage> messages)
		{
			//toido: insert to localashe
			Parallel.ForEach(messages, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, message =>
			{
			  RecievedMessage.TryUpdate(message.Id, true, false);
			});
			return Task.CompletedTask;
		}

		private void CalculateMissRate()
		{
			double missNumbers = RecievedMessage.Where(m => m.Value == false).Count();
			MissRate = (missNumbers / RecieveRange) * 100;
		}

		private void ResetStatistics()
		{
			InpuRate = 0;
		}

		private string CreateStatiustics()
		{
			return $"{InpuRate}\t{MissRate}";
		}

		/// <summary>
		/// Will Fill the Dictionary With specific Count And False Value
		/// </summary>
		/// <param name="count">Gets Total number of messages which Will Recieve</param>
		private void InitialRecievedMessages(int count)
		{
			RecievedMessage = new ConcurrentDictionary<int, bool>();
			for (int i = 1; i <= count; i++)
				RecievedMessage.TryAdd(i, false);
		}
		#endregion

		#region ISubscriberService
		public Task CheckReceivedMessageasync(List<ApiMessage> messages, [FromCap] CapHeader header)
		{
			InsertToLocalCache(messages);
			return Task.CompletedTask;
		}
		#endregion
	}
}
