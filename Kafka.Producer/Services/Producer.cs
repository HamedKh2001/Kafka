using Domain;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Producer.Application.IServices;
using System.Diagnostics;

namespace Kafka.Producer.Services
{
	public class Producer : IProducer
	{
		#region Properties
		private readonly ICapPublisher _capPublisher;
		#endregion

		#region Ctor
		public Producer(ICapPublisher capPublisher)
		{
			_capPublisher = capPublisher;
		}
		#endregion

		#region IProducer
		public async Task<bool> Publishasync(List<ApiMessage> apiMessage, int range)
		{
			try
			{
				var header = new Dictionary<string, string?>();
				header.Add("range", range.ToString());
				await _capPublisher.PublishAsync(nameof(ApiMessage), contentObj: apiMessage, headers: header);
				return await Task.FromResult(true);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return await Task.FromResult(false);
			}
		}
		#endregion
	}
}
