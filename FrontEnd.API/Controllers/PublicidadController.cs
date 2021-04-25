using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using data = FrontEnd.API.Models;


namespace FrontEnd.API.Controllers
{
    public class PublicidadController : Controller
    {
        string baseurl = "https://localhost:44374/";
        private readonly IHostingEnvironment hostingEnvironment;

        public PublicidadController(IHostingEnvironment environment)
        {
            hostingEnvironment = environment;
        }
        // GET: Publicidad
        public async Task<IActionResult> Index()
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
            return View(aux.Where(m => m.CodEmpresa == HttpContext.Session.GetInt32("CodEmpresa")));
        }

        // GET: Publicidad/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var publicidad = GetById(id);


            if (publicidad == null)
            {
                return NotFound();
            }

            return View(publicidad);
        }

        // GET: Publicidads/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Publicidads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(data.Publicidad publicidad)
        {
            string NombreArchivo = "", Ruta = "";
            //

            try
            {
                if (publicidad.Archivo != null)
                {
                    string ext = Path.GetExtension(publicidad.Archivo.FileName);
                    if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                    {
                        throw new Exception("Este tipo de archivo no es admitido. Debe ingresar un archivo en formato jpg o png.");
                    }
                    var uniqueFileName = GetUniqueFileName(publicidad.Archivo.FileName);
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    publicidad.Archivo.CopyTo(new FileStream(filePath, FileMode.Create));
                    NombreArchivo = uniqueFileName;
                    Ruta = uploads;

                    publicidad.RutaArchivo = "~/uploads/" + NombreArchivo;
                    publicidad.CodEmpresa = (int)HttpContext.Session.GetInt32("CodEmpresa");

                    if (ModelState.IsValid)
                    {
                        using (var cl = new HttpClient())
                        {

                            cl.BaseAddress = new Uri(baseurl);
                            var content = JsonConvert.SerializeObject(publicidad);
                            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                            var byteContent = new ByteArrayContent(buffer);
                            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                            var postTask = cl.PostAsync("api/Publicidad", byteContent).Result;

                            if (postTask.IsSuccessStatusCode)
                            {
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }

                }
                else
                {
                    throw new Exception("No se ha cargado una imagen válida");
                }

          
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("ErrorCrearPublicidad", ex.Message);
            }
          
            return View(publicidad);
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
        // GET: Publicidads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var publicidad = GetById(id);
            if (publicidad == null)
            {
                return NotFound();
            }
            return View(publicidad);
        }

        // POST: Publicidads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, data.Publicidad publicidad)
        {
            if (id != publicidad.CodPublicidad)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {

                    try
                    {
                        string NombreArchivo = "", Ruta = "";

                        if (publicidad.Archivo != null)
                        {
                            string ext = Path.GetExtension(publicidad.Archivo.FileName);
                            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                            {
                                throw new Exception("Este tipo de archivo no es admitido. Debe ingresar un archivo en formato jpg o png.");
                            }

                            var uniqueFileName = GetUniqueFileName(publicidad.Archivo.FileName);
                            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                            var filePath = Path.Combine(uploads, uniqueFileName);
                            publicidad.Archivo.CopyTo(new FileStream(filePath, FileMode.Create));
                            NombreArchivo = uniqueFileName;
                            Ruta = uploads;

                            publicidad.RutaArchivo = "~/uploads/" + NombreArchivo;
                            publicidad.CodEmpresa = (int)HttpContext.Session.GetInt32("CodEmpresa");


                        }

                        using (var cl = new HttpClient())
                        {
                            publicidad.CodEmpresa = (int)HttpContext.Session.GetInt32("CodEmpresa");
                            cl.BaseAddress = new Uri(baseurl);
                            var content = JsonConvert.SerializeObject(publicidad);
                            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                            var byteContent = new ByteArrayContent(buffer);
                            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                            var postTask = cl.PutAsync("api/Publicidad/" + id, byteContent).Result;

                            if (postTask.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Index");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "Este tipo de archivo no es admitido. Debe ingresar un archivo en formato jpg o png.")
                        {
                            ModelState.AddModelError("ErrorEditarPublicidad", ex.Message);
                        }
                        else
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
                        
                      
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(Exception ex)
            {
               
            }


         
           

            return View();
        }

        // GET: Publicidads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicidad = GetById(id);
            if (publicidad == null)
            {
                return NotFound();
            }

            return View(publicidad);
        }

        // POST: Publicidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await cl.DeleteAsync("api/Publicidad/" + id);

                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PublicidadExists(int id)
        {
            return (GetById(id) != null);
        }

        private data.Publicidad GetById(int? id)
        {
            data.Publicidad aux = new data.Publicidad();
            using (var cl = new HttpClient())
            {
                cl.BaseAddress = new Uri(baseurl);
                cl.DefaultRequestHeaders.Clear();
                cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = cl.GetAsync("api/Publicidad/" + id).Result;

                if (res.IsSuccessStatusCode)
                {
                    var auxres = res.Content.ReadAsStringAsync().Result;
                    aux = JsonConvert.DeserializeObject<data.Publicidad>(auxres);
                }
            }
            return aux;
        }
    }
}