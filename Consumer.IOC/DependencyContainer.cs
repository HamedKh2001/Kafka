using Consomer.Application.IService;
using Kafka.Consumer.Services;
using Microsoft.Extensions.DependencyInjection;
using Producer.Application;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Consumer.IOC
{
	public class DependencyContainer
	{
		public static void RegisterServices(IServiceCollection services)
		{
			//Infra
			services.AddSingleton<ISubscriberService, SubscriberService>();

		}
	}
}
