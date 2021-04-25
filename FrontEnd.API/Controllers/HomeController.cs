using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FrontEnd.API.Models;
using System.Net.Http;
using data = FrontEnd.API.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace FrontEnd.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        string baseurl = "https://localhost:44374/";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize(Roles = "Empresa")]
     
        public IActionResult Index()
        {
            //ViewData["NombreCompleto"] = HttpContext.User.Identity.Name;
            
            
            return View();
        }


        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> PrincipalUsuario()
        {
            List<data.Publicidad> aux = new List<data.Publicidad>();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.GetAsync("api/Publicidad");

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<List<data.Publicidad>>(auxres);

                }
            }
            return View(aux);
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
