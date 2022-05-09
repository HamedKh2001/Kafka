using Domain;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Application.Jobs
{
	public class MessageFactory:IJob
	{
		public Task Execute(IJobExecutionContext context)
		{

			var rnd = new Random();
			var message = new ApiMessage
			{
				Id = rnd.Next(1, 500),
				Timespan = TimeSpan.FromSeconds(60),
				Value = rnd.Next(),
			};
			return Task.FromResult(message);
		}
	}
}
