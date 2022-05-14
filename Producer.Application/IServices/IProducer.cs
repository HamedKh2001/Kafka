using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Application.IServices
{
	public interface IProducer
	{
		Task Publisherasync();
	}
}
