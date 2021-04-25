using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using data = FrontEnd.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace FrontEnd.API.Controllers
{
    public class ControlAforoController : Controller
    {
        string baseurl = "https://localhost:44374/";



        [Authorize(Roles = "Empresa")]
        public async Task<IActionResult> Index()
        {
            List<data.ControlAforo> aux = new List<data.ControlAforo>();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.GetAsync("api/ControlAforo");

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    
                    aux = JsonConvert.DeserializeObject<List<data.ControlAforo>>(auxres);
                    

                }
            }
            return View(aux.Where(m => m.CodEmpresa == HttpContext.Session.GetInt32("CodEmpresa")));
        }


        [Authorize(Roles = "Empresa")]
        // GET: ControlAforoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boleterias = GetById(id);


            if (boleterias == null)
            {
                return NotFound();
            }

            return View(boleterias);
        }



        [Authorize(Roles = "Empresa")]
        // GET: ControlAforoes/Create
        public IActionResult Create()
        {
           
            ViewData["Dias"] = new SelectList(getDias(), "NumeroDia", "Descripcion");
            return View();
        }

        // POST: ControlAforoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Empresa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodControl,CodEmpresa,NumeroDia,NumeroAforo")] data.ControlAforo controlAforo)
        {
            if (ModelState.IsValid)
            {
                using (var cl = new HttpClient())
                {
                    controlAforo.CodEmpresa = (int)HttpContext.Session.GetInt32("CodEmpresa");
                    cl.BaseAddress = new Uri(baseurl);
                    var content = JsonConvert.SerializeObject(controlAforo);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var postTask = cl.PostAsync("api/ControlAforo", byteContent).Result;

                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            ViewData["Dias"] = new SelectList(getDias(), "NumeroDia", "Descripcion", controlAforo.NumeroDia);
  
            return View(controlAforo);
        }

        // GET: ControlAforoes/Edit/5
        [Authorize(Roles = "Empresa")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var controlaforo = GetById(id);
            if (controlaforo == null)
            {
                return NotFound();
            }

            ViewData["Dias"] = new SelectList(getDias(), "NumeroDia", "Descripcion", controlaforo.NumeroDia);
            return View(controlaforo);
        }

        // POST: ControlAforoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empresa")]
        public async Task<IActionResult> Edit(int id, [Bind("CodControl,CodEmpresa,NumeroDia,NumeroAforo")] data.ControlAforo controlAforo)
        {
            if (id != controlAforo.CodControl)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    controlAforo.CodEmpresa = (int)HttpContext.Session.GetInt32("CodEmpresa");
                    using (var cl = new HttpClient())
                    {
                        cl.BaseAddress = new Uri(baseurl);
                        var content = JsonConvert.SerializeObject(controlAforo);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        var postTask = cl.PutAsync("api/ControlAforo/" + id, byteContent).Result;

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

            //ViewData["CodEmpresa"] = new SelectList(getAllEmpresa(), "CodEmpresa", "Nombre", boleteria.CodEmpresa);
            return View(controlAforo);
        }

        // GET: ControlAforoes/Delete/5
        [Authorize(Roles = "Empresa")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controlaforo = GetById(id);
            if (controlaforo == null)
            {
                return NotFound();
            }

            return View(controlaforo);
        }

        // POST: ControlAforoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empresa")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var cl = new HttpClient())
            {
               
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.DeleteAsync("api/ControlAforo/" + id + "?codControl=" + id);

                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Empresa")]
        private bool ControlAforoExists(int id)
        {
            return (GetById(id) != null);
        }


        [Authorize(Roles = "Empresa")]
        private data.ControlAforo GetById(int? id)
        {
            data.ControlAforo aux = new data.ControlAforo();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = cl.GetAsync("api/ControlAforo/" + id + "?CodControl=" + id).Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<data.ControlAforo>(auxres);
                }
            }
            return aux;
        }

        public class DiasSemana
        {
            public int NumeroDia { get; set; }
            public string Descripcion { get; set; }

            public DiasSemana(int NumeroDia, string Descripcion)
            {
                this.NumeroDia = NumeroDia;
                this.Descripcion = Descripcion;
            }
        }

        private List<DiasSemana> getDias()
        {

            List<DiasSemana> Dias = new List<DiasSemana>();
            Dias.Add(new DiasSemana(1, "Lunes"));
            Dias.Add(new DiasSemana(2, "Martes"));
            Dias.Add(new DiasSemana(3, "Miércoles"));
            Dias.Add(new DiasSemana(4, "Jueves"));
            Dias.Add(new DiasSemana(5, "Viernes"));
            Dias.Add(new DiasSemana(6, "Sabado"));
            Dias.Add(new DiasSemana(7, "Domingo"));
            return Dias;
        }





    }
}