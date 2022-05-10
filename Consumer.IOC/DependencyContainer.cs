using Consomer.Application.IService;
using Kafka.Consumer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Consumer.IOC
{
	public class DependencyContainer
	{
		public static void RegisterServices(IServiceCollection services)
		{
			//Infra
			services.AddScoped<ISubscriberService, SubscriberService>();
		}
	}
}
