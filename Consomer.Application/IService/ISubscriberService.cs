using Domain;
using DotNetCore.CAP;

namespace Consomer.Application.IService
{
	public interface ISubscriberService: ICapSubscribe
	{
		Task CheckReceivedMessageasync(List<ApiMessage> messages, [FromCap] CapHeader header);
	}
}
