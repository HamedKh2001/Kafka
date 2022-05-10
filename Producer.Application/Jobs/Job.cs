using Quartz;

namespace Producer.Application.Jobs
{
	public class Job : IJob
	{
		#region Properties
		private IServices.IProducer _producer { get; set; }
		#endregion

		#region Ctor
		public Job(IServices.IProducer producer)
		{
			_producer = producer;
		}
		#endregion

		#region IJob
		public Task Execute(IJobExecutionContext context)
		{
			MessageFactory factory = new(100);
			var message = factory.GenerateMessage();
			var res = _producer.Publish(message, factory.range).Result;
			return Task.CompletedTask;
		}
		#endregion
	}
}
