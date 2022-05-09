using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Application.Jobs
{
	public class Job : IJob
	{
		private IServices.IProducer _producer { get; set; }
		public Job(IServices.IProducer producer)
		{
			_producer = producer;
		}

		public Task Execute(IJobExecutionContext context)
		{
			MessageFactory factory = new();
			var message = factory.GenerateMessage();
			var red = _producer.Publish(message).Result;
			return Task.CompletedTask;
		}
	}
}
