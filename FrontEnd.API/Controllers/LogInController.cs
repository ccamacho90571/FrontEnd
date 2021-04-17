using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using FrontEnd.API.Tools;
using data = FrontEnd.API.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace FrontEnd.API.Controllers
{
    public class LogInController : Controller
    {
        data.Usuarios usuario = new data.Usuarios();
        string Key = "crbda58907094133bbce2ea205081916";
        string baseurl = "https://localhost:44374/";
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LogIn(data.Usuarios User)
        {
            var secretkey = Key;
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                else
                {
                    data.Usuarios aux = new data.Usuarios();
                    using (var cl = new HttpClient())
                    {
                        cl.BaseAddress = new Uri(baseurl);
                        cl.DefaultRequestHeaders.Clear();
                        cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        User.Contrasena = Seguridad.EncryptString(Key, User.Contrasena);
                        User.Usuario = User.Usuario.ToLower();
                        HttpResponseMessage res = await cl.GetAsync("api/Usuarios/" + User.Usuario + "/" + User.Contrasena + "?usuario=" + User.Usuario);

                        if (res.IsSuccessStatusCode)
                        {
                            var auxres = res.Content.ReadAsStringAsync().Result;
                            aux = JsonConvert.DeserializeObject<data.Usuarios>(auxres);

                        }

                        if (aux.Usuario == null)
                        {
                            ModelState.AddModelError("Error", "Usuario o contraseña son incorrectos");
                            return View();
                        }
                        else
                        {
                            if (aux.Usuario == "Admin")
                            {

                            }
                            else if (aux.Tipo)
                            {

                                //HttpContext.Session.SetString("NombreCompleto", aux.Nombre);
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarUser(data.Usuarios User)
        {
            var secretkey = Key;
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                else
                {
                    string cont = RetornarContrasena();
                    User.Tipo = false;
                    User.Contrasena = Seguridad.EncryptString(Key, cont);
                    User.Nombre = User.Nombre.ToLower();
                    using (var cl = new HttpClient())
                    {
                        cl.BaseAddress = new Uri(baseurl);
                        var content = JsonConvert.SerializeObject(User);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        var postTask = cl.PostAsync("api/Usuarios", byteContent).Result;

                        if (postTask.IsSuccessStatusCode)
                        {
                            string body = CambiarCorreo(User.Usuario, cont);
                            Correo.EnviarCorreo(User.Correo, "Nuevo registro", body);


                            return RedirectToAction("LogIn", "LogIn");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        protected string RetornarContrasena()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            string finalString = new String(stringChars);
            return finalString;

        }

        protected string CambiarCorreo(string Usuario, string Contrasena)
        {
            string Body = System.IO.File.ReadAllText("../FrontEnd.API/Tools/Plantillas_Correo/Correo_NuevoUsuario.html");

            Body = Body.Replace("[Usuario]", Usuario);

            Body = Body.Replace("[Contrasena]", Contrasena);

            return Body;





        }

    }
}