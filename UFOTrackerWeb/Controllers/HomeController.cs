using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UFOTrackerWeb.Models;
using UFOTracker.Models;
using Newtonsoft.Json;

namespace UFOTrackerWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static HttpClient client = new HttpClient();



        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var id = "62e2e4daaf19000045019b60";
            HttpResponseMessage response = await client.GetAsync("https://localhost:7053/Ufo/" + id);
            response.EnsureSuccessStatusCode();
            var EmpResponse = response.Content.ReadAsStringAsync().Result;
            var EmpInfo = JsonConvert.DeserializeObject<Ufo>(EmpResponse);
            //var test = _ufoService.GetOneUfo("62e2e4daaf19000045019b60");
            return View();
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}