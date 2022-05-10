using Consomer.Application.IService;
using Domain;
using DotNetCore.CAP;
using System.Diagnostics;

namespace Kafka.Consumer.Services
{
	public class SubscriberService : ISubscriberService, ICapSubscribe
	{
		[CapSubscribe("KafkaSample")]
		public void CheckReceivedMessage(List<ApiMessage> messages)
		{
			var res = messages.GroupBy(x => x.Id).ToList();
			Debug.WriteLine(messages.Count);
		}
	}
}
