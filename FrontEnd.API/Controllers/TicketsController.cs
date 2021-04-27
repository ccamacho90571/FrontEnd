using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FrontEnd.API.Tools;
using Newtonsoft.Json;
using data = FrontEnd.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace FrontEnd.API.Controllers
{
    public class TicketsController : Controller
    {
        string baseurl = "https://localhost:44374/";



        // GET: Tickets
        [Authorize(Roles = "Empresa,Usuario")]
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Tickets.ToListAsync());

            List<data.Tickets> aux = new List<data.Tickets>();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.GetAsync("api/Tickets");

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<List<data.Tickets>>(auxres);
                }
            }
            return View(aux.Where(m => m.CodEmpresa == HttpContext.Session.GetInt32("CodEmpresa")));
        }

        // GET: Tickets
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> MisReservas()
        {
            //return View(await _context.Tickets.ToListAsync());

            List<data.Tickets> aux = new List<data.Tickets>();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.GetAsync("api/Tickets");

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<List<data.Tickets>>(auxres);
                }
            }
            return View(aux.Where(m => m.Usuario == HttpContext.Session.GetString("Usuario")));
        }

        [Authorize(Roles = "Empresa")]
        public async Task<IActionResult> ValidarTicket()
        {
            data.Tickets ticket = new data.Tickets();
            return View(ticket);
        }


        [Authorize(Roles = "Empresa")]
        public async Task<IActionResult> BuscarTicket(string NReserva)
        {
            try
            {
                data.Tickets aux = new data.Tickets();
                using (var cl = new HttpClient())
                {
                    cl.BaseAddress = new Uri(baseurl);
                    cl.DefaultRequestHeaders.Clear();
                    cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                 
                    HttpResponseMessage res = cl.GetAsync("api/Tickets/Tickets/" + NReserva).Result;

                    if (res.IsSuccessStatusCode)
                    {
                        var auxres = res.Content.ReadAsStringAsync().Result;
                        aux = JsonConvert.DeserializeObject<data.Tickets>(auxres);

                        if (aux.Estado != 1 || aux.CodEmpresa != HttpContext.Session.GetInt32("CodEmpresa"))
                        {
                            throw new Exception("Esta reserva no existe");
                        }
                        return PartialView("_BuscarTicket", aux);

                    }
                    else
                    {

                        throw new Exception("Esta reserva no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ErrorReserva", ex.Message);
            }
            return View();
        }










        // GET: Tickets/Details/5
        [Authorize(Roles = "Empresa")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tickets = GetById(id);


            if (tickets == null)
            {
                return NotFound();
            }

            return View(tickets);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Usuario")]
        public IActionResult Create()
        {
            ViewData["CodEmpresas"] = new SelectList(getAllEmpresas(), "CodEmpresa", "Nombre");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> Create([Bind("CodTicket,Nreserva,Usuario,CodEmpresa,Espacio,Fecha,Estado")] data.Tickets tickets)
        {
            try
            {
                data.Empresa auxemp = GetByIdEmpresa(tickets.CodEmpresa);
              
                tickets.CodEmpresaNavigation = auxemp;
                if (tickets.Espacio > tickets.CodEmpresaNavigation.ReservasUsuario)
                {
                    throw new Exception("ErrorReservasPermitidas");
                }
                else if (getEspaciosDisponiblesDia(tickets.Fecha, tickets.CodEmpresa) < tickets.Espacio)
                {
                    throw new Exception("ErrorEspaciosDisponibles");

                }
                else if (getEspaciosDisponibles(tickets.Fecha) > tickets.Espacio)
                {
                    throw new Exception("ErrorNoEspacio");
                }
                else
                {


                    if (ModelState.IsValid)
                    {
                        using (var cl = new HttpClient())
                        {
                            tickets.Usuario = HttpContext.Session.GetString("Usuario");
                            tickets.Nreserva = "CRP" + RetornarCodigo();
                            tickets.Estado = 1;
                            data.Usuarios auxusu = GetById(tickets.Usuario);
                            cl.BaseAddress = new Uri(baseurl);
                            var content = JsonConvert.SerializeObject(tickets);
                            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                            var byteContent = new ByteArrayContent(buffer);
                            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                            var postTask = cl.PostAsync("api/Tickets", byteContent).Result;

                            if (postTask.IsSuccessStatusCode)
                            {
                                string body = CambiarCorreo(auxemp.Nombre, tickets.Espacio, tickets.Fecha);
                                Correo.EnviarCorreo(auxusu.Correo, "Reserva realizada", body);
                                ModelState.AddModelError("ExitoReserva", "Se ha reservado su espacio con éxito.");

                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                if (ex.Message.Equals("ErrorReservasPermitidas"))
                {
                    ModelState.AddModelError("ErrorReservasPermitidas", "Se ha excedido la cantidad de reservas permitidas por usuario");
                }
                else if (ex.Message.Equals("ErrorEspaciosDisponibles"))
                {
                    ModelState.AddModelError("ErrorEspaciosDisponibles", "La cantidad supera el aforo de personas permitidas ese día");
                }
                else if (ex.Message.Equals("ErrorNoEspacio"))
                {
                    ModelState.AddModelError("ErrorNoEspacio", "No hay espacio para la fecha seleccionada");
                }
            }
          
           
            //ViewData["GroupUpdateId"] = new SelectList(GetAllGroupUpdates(), "GroupUpdateId", "GroupUpdateId", groupComment.GroupUpdateId);
            return View(tickets);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tickets = GetById(id);
            if (tickets == null)
            {
                return NotFound();
            }
            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodTicket,Nreserva,Usuario,CodEmpresa,Fecha,Estado")] data.Tickets tickets)
        {
            if (id != tickets.CodTicket)
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
                        var content = JsonConvert.SerializeObject(tickets);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        var postTask = cl.PutAsync("api/Tickets/" + id, byteContent).Result;

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
            //ViewData["GroupUpdateId"] = new SelectList(GetAllGroupUpdates(), "GroupUpdateId", "GroupUpdateId", groupComment.GroupUpdateId);
            return View(tickets);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tickets = GetById(id);
            if (tickets == null)
            {
                return NotFound();
            }

            return View(tickets);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.DeleteAsync("api/Tickets/" + id);

                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TicketsExists(int id)
        {
            return (GetById(id) != null);
        }

        private data.Tickets GetById(int? id)
        {
            data.Tickets aux = new data.Tickets();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage res = await cl.GetAsync("api/Pais/5?"+id);
                HttpResponseMessage res = cl.GetAsync("api/Tickets/" + id).Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<data.Tickets>(auxres);
                }
            }
            return aux;
        }

        private data.Usuarios GetById(string usuario)
        {
            data.Usuarios aux = new data.Usuarios();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage res = await cl.GetAsync("api/Pais/5?"+id);
                HttpResponseMessage res = cl.GetAsync("api/Usuarios/" + usuario).Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<data.Usuarios>(auxres);
                }
            }
            return aux;
        }

        private data.Empresa GetByIdEmpresa(int? id)
        {
            data.Empresa aux = new data.Empresa();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage res = await cl.GetAsync("api/Pais/5?"+id);
                HttpResponseMessage res = cl.GetAsync("api/Empresa/" + id).Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<data.Empresa>(auxres);
                }
            }
            return aux;
        }


        protected string RetornarCodigo()
        {
            bool valida = true;
            string finalString = "";
            do
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var stringChars = new char[3];
                var random = new Random();
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                finalString = new String(stringChars);

         

                using (var cl = new HttpClient())
                {
                    cl.BaseAddress = new Uri(baseurl);
                    cl.DefaultRequestHeaders.Clear();
                    cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage res = cl.GetAsync("api/Tickets/Tickets/" + "CRP" + finalString).Result;

                    if (res.IsSuccessStatusCode)
                    {
                        valida = false;
                    }
                    
                }
            } while (!valida);
          
           return finalString;

        }
        private List<data.Empresa> getAllEmpresas()
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

        private int getEspaciosDisponibles(DateTime Fecha)
        {
            int q = 0;
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
            
            q = aux.Where(m => m.Fecha == Fecha.Date).Sum(m => m.Espacio);

            return q;
        }
        protected string CambiarCorreo(string Lugar, int Espacios, DateTime Fecha)
        {
            string Body = System.IO.File.ReadAllText("../FrontEnd.API/Tools/Plantillas_Correo/Correo_InfoReserva.html");

            Body = Body.Replace("[FECHA]", Fecha.ToShortDateString());

            Body = Body.Replace("[ESPACIOS]", Espacios.ToString());

            Body = Body.Replace("[NombreLugar]", Lugar);

            return Body;

        }
        private int getEspaciosDisponiblesDia(DateTime Fecha, int CodEmpresa)
        {
            int q = 0;
            int nday = (int)Fecha.DayOfWeek;
            if (nday == 0)
            {
                nday = 7;
            }
            List<data.ControlAforo> aux = new List<data.ControlAforo>();
            data.ControlAforo obj = new data.ControlAforo();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = cl.GetAsync("api/ControlAforo").Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<List<data.ControlAforo>>(auxres);
                }

               
            }
            obj = aux.SingleOrDefault(m => m.CodEmpresa == CodEmpresa && m.NumeroDia == nday);
            
            
            return obj.NumeroAforo;
        }

    }
}
