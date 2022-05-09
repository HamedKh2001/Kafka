using Domain;

namespace Producer.Application.IServices
{
	public interface IProducer
	{
		Task<bool> Publish(ApiMessage apiMessage);
	}
}
