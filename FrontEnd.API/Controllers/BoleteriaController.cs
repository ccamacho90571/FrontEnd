using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using data = FrontEnd.API.Models;

namespace FrontEnd.API.Controllers
{
    public class BoleteriaController : Controller
    {
        string baseurl = "https://localhost:44374/";




        [Authorize(Roles ="Empresa")]
        // GET: Boleterias
        public async Task<IActionResult> Index()
        {
            List<data.Boleteria> aux = new List<data.Boleteria>();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.GetAsync("api/Boleteria");

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<List<data.Boleteria>>(auxres);
                    
                }
            }
            return View(aux.Where(m => m.CodEmpresa == HttpContext.Session.GetInt32("CodEmpresa")));
        }


        [Authorize(Roles = "Empresa")]
        // GET: Boleterias/Details/5
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
        // GET: Boleterias/Create
        public IActionResult Create()
        {
            ViewData["CodEmpresa"] = new SelectList(getAllEmpresa(), "CodEmpresa", "Nombre");
            return View();
        }

        // POST: Boleterias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Empresa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodBoleteria,CodEmpresa,Descripcion,Costo")] data.Boleteria boleteria)
        {
            if (ModelState.IsValid)
            {
                using (var cl = new HttpClient())
                {
                    boleteria.CodEmpresa = (int)HttpContext.Session.GetInt32("CodEmpresa");
                    cl.BaseAddress = new Uri(baseurl);
                    var content = JsonConvert.SerializeObject(boleteria);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var postTask = cl.PostAsync("api/Boleteria", byteContent).Result;

                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

           
            return View(boleteria);
        }

        [Authorize(Roles = "Empresa")]
        // GET: Boleterias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var boleteria = GetById(id);
            if (boleteria == null)
            {
                return NotFound();
            }

           
            return View(boleteria);
        }

        // POST: Boleterias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Empresa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodBoleteria,CodEmpresa,Descripcion,Costo")] data.Boleteria boleteria)
        {
            if (id != boleteria.CodBoleteria)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var cl = new HttpClient())
                    {
                        boleteria.CodEmpresa = (int)HttpContext.Session.GetInt32("CodEmpresa");
                        cl.BaseAddress = new Uri(baseurl);
                        var content = JsonConvert.SerializeObject(boleteria);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        var postTask = cl.PutAsync("api/Boleteria/" + id, byteContent).Result;

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

           
            return View(boleteria);
        }

        [Authorize(Roles = "Empresa")]
        // GET: Boleterias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boleteria = GetById(id);
            if (boleteria == null)
            {
                return NotFound();
            }

            return View(boleteria);
        }

        // POST: Boleterias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.DeleteAsync("api/Boleteria/" + id);

                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Empresa")]
        private bool BoleteriaExists(int id)
        {
            return (GetById(id) != null);
        }
        [Authorize(Roles = "Empresa")]
        private data.Boleteria GetById(int? id)
        {
            data.Boleteria aux = new data.Boleteria();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = cl.GetAsync("api/Boleteria/" + id).Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<data.Boleteria>(auxres);
                }
            }
            return aux;
        }
        [Authorize(Roles = "Empresa")]
        private List<data.Empresa> getAllEmpresa()
        {

            List<data.Empresa> aux = new List<data.Empresa>();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = cl.GetAsync("api/Empresa").Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<List<data.Empresa>>(auxres);
                }
            }
            return aux;
        }
    }
}
