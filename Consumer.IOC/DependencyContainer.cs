using Consomer.Application.IService;
using Kafka.Consumer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Consumer.IOC
{
	public class DependencyContainer
	{
		public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddTransient<ISubscriberService, SubscriberService>();


			#region NLog
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
