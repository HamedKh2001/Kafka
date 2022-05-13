using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Producer.Application;
using Producer.Application.IServices;
using Producer.Application.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Producer.IOC
{
	public class DependencyContainer
	{
		public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
		{
			//Application
			services.AddSingleton<IProducer, Kafka.Producer.Services.Producer>();

			//Infra


			#region CAP
			services.AddCap(x =>
			{
				x.UseInMemoryMessageQueue();
				x.UseInMemoryStorage();
				//todo: add to config
				x.UseKafka(configuration.GetSection("KafkaConfigs")["Bootstrapserver"]);
			});
			#endregion

			#region Quartz
			services.AddSingleton<IJobFactory, SingletonJobFactory>();
			services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

			services.AddSingleton<ProducerJob>();
			services.AddSingleton(new JobSchedule(jobType: typeof(ProducerJob),
				cronExpression: configuration.GetSection("QuartzConfig")["CronExpression"]));
			services.AddHostedService<QuartzHostedService>();
			#endregion
		}
	}
}
