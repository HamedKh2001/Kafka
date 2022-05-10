using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;

namespace Producer.Application
{
	public class QuartzHostedService : IHostedService
	{

		#region Properties
		private readonly ISchedulerFactory _schedulerFactory;
		private readonly IJobFactory _jobFactory;
		private readonly IEnumerable<JobSchedule> _jobSchedules;
		public IScheduler Scheduler { get; set; }
		#endregion

		#region Ctor
		public QuartzHostedService(
			ISchedulerFactory schedulerFactory,
			IJobFactory jobFactory,
			IEnumerable<JobSchedule> jobSchedules)
		{
			_schedulerFactory = schedulerFactory;
			_jobSchedules = jobSchedules;
			_jobFactory = jobFactory;
		}
		#endregion


		#region IHostedService
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
			Scheduler.JobFactory = _jobFactory;

			foreach (var jobSchedule in _jobSchedules)
			{
				var job = CreateJob(jobSchedule);
				var trigger = CreateTrigger(jobSchedule);

				await Scheduler.ScheduleJob(job, trigger, cancellationToken);
			}

			await Scheduler.Start(cancellationToken);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await Scheduler?.Shutdown(cancellationToken);
		}
		#endregion

		#region HelperMethod
		private static IJobDetail CreateJob(JobSchedule schedule)
		{
			var jobType = schedule.JobType;
			return JobBuilder
				.Create(jobType)
				.WithIdentity(jobType.FullName)
				.WithDescription(jobType.Name)
				.Build();
		}

		private static ITrigger CreateTrigger(JobSchedule schedule)
		{
			return TriggerBuilder
				.Create()
				.WithIdentity($"{schedule.JobType.FullName}.trigger")
				.WithCronSchedule(schedule.CronExpression)
				.WithDescription(schedule.CronExpression)
				.Build();
		}
		#endregion
	}
}
