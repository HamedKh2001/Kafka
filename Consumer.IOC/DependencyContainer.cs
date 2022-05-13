using Consomer.Application.IService;
using Kafka.Consumer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Producer.Application;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Consumer.IOC
{
	public class DependencyContainer
	{
		public static void RegisterServices(IServiceCollection services,IConfiguration configuration)
		{
			//Infra
			services.AddSingleton<ISubscriberService, SubscriberService>();


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
