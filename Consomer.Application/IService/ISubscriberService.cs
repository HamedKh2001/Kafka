using Domain;
using DotNetCore.CAP;

namespace Consomer.Application.IService
{
	public interface ISubscriberService
	{
		Task CheckReceivedMessage(List<ApiMessage> messages, [FromCap] CapHeader header);
	}
}
