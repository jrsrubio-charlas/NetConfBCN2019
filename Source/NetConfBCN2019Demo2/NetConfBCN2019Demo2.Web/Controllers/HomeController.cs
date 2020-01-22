using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetConfBCN2019Demo2.CrossCutting.Models;
using NetConfBCN2019Demo2.Web.Models;
using Newtonsoft.Json;

namespace NetConfBCN2019Demo2.Web.Controllers
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
            return View("Index");
        }

        public IActionResult Results()
        {            
            return View(
                new EndPointsSettingsModel {
                    ApiURL = _config.Value.ApiURL,
                    FunctionURL = _config.Value.FunctionURL }
                );
        }

        [HttpPost]
        public async Task<IActionResult> Vote(VoteRawModel vote)
        {
            string postBody = JsonConvert.SerializeObject(vote);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var json = await httpClient.PostAsync(_config.Value.ApiURL, new StringContent(postBody, Encoding.UTF8, "application/json"));

            if (json.IsSuccessStatusCode)
            {
                return RedirectToAction("Results","Home");
            }
            else
            {
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = "Something was wrong" });
            }
        }

    }
}
