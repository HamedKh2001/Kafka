using Domain;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Consumer.Controllers
{
    [Controller]
    public class PublishController : Controller
    {
        [CapSubscribe("tutorial")]
        public void CheckReceivedMessage(ApiMessage apiMessage)
        {
            Debug.WriteLine(apiMessage.DateTime);
        }
    }
}
