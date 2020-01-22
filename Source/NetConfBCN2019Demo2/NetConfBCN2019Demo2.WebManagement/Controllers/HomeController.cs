using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetConfBCN2019Demo2.WebManagement.Models;

namespace NetConfBCN2019Demo2.WebManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<EndPointsSettingsModel> _config;

        public HomeController(IOptions<EndPointsSettingsModel> config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            ViewData["FunctionUrl"] = _config.Value.FunctionURL;

            return View();           
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
