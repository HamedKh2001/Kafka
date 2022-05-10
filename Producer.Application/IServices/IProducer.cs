using Domain;
using DotNetCore.CAP;

namespace Producer.Application.IServices
{
	public interface IProducer
	{
		public string topic { get; }
		Task<bool> Publish(List<ApiMessage> apiMessage, int range);
	}
}
