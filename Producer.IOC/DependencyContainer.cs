using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nlog.Logging;
using Producer.Application.IServices;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Producer.IOC
{
	public class DependencyContainer
	{
		public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<IProducer, Kafka.Producer.Services.Producer>();

			#region CAP
			services.AddCap(x =>
			{
				x.UseInMemoryMessageQueue();
				x.UseInMemoryStorage();
				//todo: add to config
				x.UseKafka(configuration.GetSection("KafkaConfigs")["Bootstrapserver"]);
			});
			#endregion

			#region NLog
			services.AddTransient(typeof(ILogger<>), typeof(NLogAdapter<>));
			services.AddTransient<Microsoft.AspNetCore.Http.IHttpContextAccessor,
				Microsoft.AspNetCore.Http.HttpContextAccessor>();
			#endregion

			#region Quartz
			//services.AddSingleton<IJobFactory, SingletonJobFactory>();
			//services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

			//services.AddSingleton<ProducerJob>();
			//services.AddSingleton(new JobSchedule(jobType: typeof(ProducerJob),
			//	cronExpression: configuration.GetSection("QuartzConfig")["CronExpression"]));
			//services.AddHostedService<QuartzHostedService>();
			#endregion
		}
	}
}
