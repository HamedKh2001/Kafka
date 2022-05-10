using Domain;

namespace Consomer.Application.IService
{
	public interface ISubscriberService
	{
		void CheckReceivedMessage(List<ApiMessage> messages);
	}
}
