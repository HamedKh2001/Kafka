using Consomer.Application.IService;
using Kafka.Consumer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nlog.Logging;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Consumer.IOC
{
	public class DependencyContainer
	{
		public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<ISubscriberService, SubscriberService>();

			#region NLog
			services.AddTransient(typeof(ILogger<>),typeof(NLogAdapter<>));
			services.AddTransient<Microsoft.AspNetCore.Http.IHttpContextAccessor,
				Microsoft.AspNetCore.Http.HttpContextAccessor>();
			#endregion

			#region Log4net
			//services.AddTransient(serviceType: typeof(NLogging.ILogger<>),implementationType: typeof(NLogging.NLogAdapter<>));
			#endregion

			#region CAP
			services.AddCap(x =>
			{
				x.UseInMemoryMessageQueue();
				x.UseInMemoryStorage();
				x.UseKafka(configuration.GetSection("KafkaConfigs")["BootstrapServer"]);
			});
			#endregion
		}
	}
}
