using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ExpensesTracker.Web.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace ExpensesTracker.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task< IActionResult> Index()
        {
            List<Budget> students = new List<Budget>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60375/api/");
                //HTTP GET
                var responseTask = client.GetAsync("budget");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();

                    students = JsonConvert.DeserializeObject<List<Budget>>(readTask); ;// readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    students = new List<Budget>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(students);

        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new List<Budget> { new Budget { Name = "TESt" } }.Where(c=>c.Name!=null));
        }
    }
}
