using Microsoft.AspNetCore.Mvc;
using Producer.Application.IServices;

namespace Producer.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ProducerController : ControllerBase
	{
		#region Properties
		private IProducer _producer;
		#endregion

		#region Ctor
		public ProducerController(IProducer producer)
		{
			_producer = producer;
		}
		#endregion

		#region Controllers
		[HttpGet]
		public async Task<IActionResult> Send()
		{
			await _producer.Publisherasync();
			return await Task.FromResult(Ok());
		}
		#endregion
	}
}
