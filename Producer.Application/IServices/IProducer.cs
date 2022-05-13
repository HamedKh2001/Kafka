using Domain;
using DotNetCore.CAP;

namespace Producer.Application.IServices
{
	public interface IProducer
	{
		Task<bool> Publishasync(List<ApiMessage> apiMessage, Dictionary<string, string?> header);
	}
}
