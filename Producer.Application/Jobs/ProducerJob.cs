using Microsoft.Extensions.Configuration;
using Quartz;

namespace Producer.Application.Jobs
{
	public class ProducerJob : IJob
	{
		#region Properties
		private IServices.IProducer _producer { get; set; }
		private int CountPerStep;
		private int Range { get; }
		#endregion

		#region DI
		private readonly IConfiguration _configuration;
		#endregion

		#region Ctor
		public ProducerJob(IServices.IProducer producer, IConfiguration configuration)
		{
			_producer = producer;
			_configuration = configuration;
			Range = Convert.ToInt32(_configuration.GetSection("ProducerConfig")["Range"]);
			CountPerStep = Convert.ToInt32(_configuration.GetSection("ProducerConfig")["CountPerStep"]);
		}
		#endregion

		#region IJob
		public Task Execute(IJobExecutionContext context)
		{
			MessageFactory factory = new(Range);
			var message = factory.GenerateMessage();
			var guid = Guid.NewGuid();
			var newguid = guid.ToString().Split('-')[0];
			for (int i = 0; i < message.Count / CountPerStep; i++)
			{
				var header = new Dictionary<string, string?>();
				header.Add("range", factory.Range.ToString());
				header.Add("identifier", newguid);
				var publishableMessage = message.Skip(i * CountPerStep).Take(CountPerStep);
				var res = _producer.Publishasync(publishableMessage.ToList(), header).Result;
				//Task.Delay(400);
			}
			return Task.CompletedTask;
		}
		#endregion
	}
}