using client.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace client.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        HttpClient client = new HttpClient();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            client.BaseAddress = new Uri("https://localhost:7145");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:7145/api/files/all");

            var re = await response.Content.ReadFromJsonAsync<ICollection<FileModel>>();

            return View(re);
        }
    }
}