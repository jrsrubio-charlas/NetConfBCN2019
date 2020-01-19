using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NetConfBCN2019Demo1.Hubs;

namespace NetConfBCN2019Demo1.Controllers
{
    public class HomeController : Controller
    {
        IHubContext<MessageHub> context;
        public HomeController(IHubContext<MessageHub> hub)
        {
            context = hub;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Message()
        {
            return View();
        }

        public void AddMessage(string message)
        {
            context.Clients.All.SendAsync("new-message", message);         
        }

        
    }
}
