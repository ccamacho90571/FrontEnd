using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using data = FrontEnd.API.Models;

namespace FrontEnd.API.Controllers
{
    public class BoleteriaReservadosController : Controller
    {
        string baseurl = "https://localhost:44374/";

        // GET: BoleteriaReservados
        public async Task<IActionResult> Index()
        {
            List<data.BoleteriaReservados> aux = new List<data.BoleteriaReservados>();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.GetAsync("api/BoleteriaReservados");

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<List<data.BoleteriaReservados>>(auxres);
                }
            }
            return View(aux);
        }

        // GET: BoleteriaReservados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boleteriaReservados = GetById(id);


            if (boleteriaReservados == null)
            {
                return NotFound();
            }

            return View(boleteriaReservados);
        }

        // GET: BoleteriaReservados/Create
        public IActionResult Create()
        {
            ViewData["CodBoleteria"] = new SelectList(getAllBoleteria(), "CodBoleteria", "Descripcion");
            ViewData["CodTickets"] = new SelectList(getAllTickets(), "CodTicket", "Nreserva");
            return View();
        }

        // POST: BoleteriaReservados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodBoletaReservado,CodBoleteria,CodTickets,Cantidad")] data.BoleteriaReservados boleteriaReservados)
        {
            if (ModelState.IsValid)
            {
                using (var cl = new HttpClient())
                {
                    cl.BaseAddress = new Uri(baseurl);
                    var content = JsonConvert.SerializeObject(boleteriaReservados);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var postTask = cl.PostAsync("api/BoleteriaReservados", byteContent).Result;

                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            ViewData["CodBoleteria"] = new SelectList(getAllBoleteria(), "CodBoleteria", "Descripcion", boleteriaReservados.CodBoleteria);
            ViewData["CodTickets"] = new SelectList(getAllTickets(), "CodTicket", "Nreserva", boleteriaReservados.CodTickets);
            return View(boleteriaReservados);
        }

        // GET: BoleteriaReservados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var boleteriaReservados = GetById(id);
            if (boleteriaReservados == null)
            {
                return NotFound();
            }

            ViewData["CodBoleteria"] = new SelectList(getAllBoleteria(), "CodBoleteria", "Descripcion", boleteriaReservados.CodBoleteria);
            ViewData["CodTickets"] = new SelectList(getAllTickets(), "CodTicket", "Nreserva", boleteriaReservados.CodTickets);
            return View(boleteriaReservados);
        }

        // POST: BoleteriaReservados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodBoletaReservado,CodBoleteria,CodTickets,Cantidad")] data.BoleteriaReservados boleteriaReservados)
        {
            if (id != boleteriaReservados.CodBoletaReservado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var cl = new HttpClient())
                    {
                        cl.BaseAddress = new Uri(baseurl);
                        var content = JsonConvert.SerializeObject(boleteriaReservados);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        var postTask = cl.PutAsync("api/BoleteriaReservados/" + id, byteContent).Result;

                        if (postTask.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
                catch (Exception)
                {
                    var aux2 = GetById(id);
                    if (aux2 == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CodBoleteria"] = new SelectList(getAllBoleteria(), "CodBoleteria", "Descripcion", boleteriaReservados.CodBoleteria);
            ViewData["CodTickets"] = new SelectList(getAllTickets(), "CodTicket", "Nreserva", boleteriaReservados.CodTickets);
            return View(boleteriaReservados);
        }

        // GET: BoleteriaReservados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boleteriaReservados = GetById(id);
            if (boleteriaReservados == null)
            {
                return NotFound();
            }

            return View(boleteriaReservados);
        }

        // POST: BoleteriaReservados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.DeleteAsync("api/BoleteriaReservados/" + id);

                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BoleteriaReservadosExists(int id)
        {
            return (GetById(id) != null);
        }

        private data.BoleteriaReservados GetById(int? id)
        {
            data.BoleteriaReservados aux = new data.BoleteriaReservados();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = cl.GetAsync("api/BoleteriaReservados/" + id).Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<data.BoleteriaReservados>(auxres);
                }
            }
            return aux;
        }

        private List<data.Boleteria> getAllBoleteria()
        {

            List<data.Boleteria> aux = new List<data.Boleteria>();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = cl.GetAsync("api/Boleteria").Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<List<data.Boleteria>>(auxres);
                }
            }
            return aux;
        }


        private List<data.Tickets> getAllTickets()
        {

            List<data.Tickets> aux = new List<data.Tickets>();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = cl.GetAsync("api/Tickets").Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<List<data.Tickets>>(auxres);
                }
            }
            return aux;
        }
    }
}
