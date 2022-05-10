using Microsoft.Extensions.DependencyInjection;
using Producer.Application;
using Producer.Application.IServices;
using Producer.Application.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Producer.IOC
{
	public class DependencyContainer
	{
		public static void RegisterServices(IServiceCollection services)
		{
			//Application
			services.AddSingleton<IProducer, Kafka.Producer.Services.Producer>();

			//Infra


			#region Quartz
			services.AddSingleton<IJobFactory, SingletonJobFactory>();
			services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

			services.AddSingleton<Job>();
			//services.AddSingleton(new JobSchedule(jobType: typeof(Job), cronExpression: "* * * ? * *"));
			services.AddSingleton(new JobSchedule(jobType: typeof(Job), cronExpression: "0 * * ? * *"));
			services.AddHostedService<QuartzHostedService>();
			#endregion
		}
	}
}
