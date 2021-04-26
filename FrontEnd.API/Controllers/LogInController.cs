using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using FrontEnd.API.Tools;
using data = FrontEnd.API.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

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

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("LogIn");
        }

        public IActionResult CambiarContrasena()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CambiarContrasena(data.Usuarios User)
        {
            try
            {
                User.Usuario = HttpContext.Session.GetString("Usuario");
                data.Usuarios aux = new data.Usuarios();
                using (var cl = new HttpClient())
                {
                    cl.BaseAddress = new Uri(baseurl);
                    cl.DefaultRequestHeaders.Clear();
                    cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    User.ContrasenaAnterior = Seguridad.EncryptString(Key, User.ContrasenaAnterior);
                    User.Usuario = User.Usuario.ToLower();
                    HttpResponseMessage res = await cl.GetAsync("api/Usuarios/" + User.Usuario + "/" + User.ContrasenaAnterior + "?usuario=" + User.Usuario);

                    if (res.IsSuccessStatusCode)
                    {
                        var auxres = res.Content.ReadAsStringAsync().Result;
                        aux = JsonConvert.DeserializeObject<data.Usuarios>(auxres);



                        if (User.Contrasena == User.Contrasena2)
                        {
                            aux.Contrasena = Seguridad.EncryptString(Key, User.Contrasena2);
                            using (var cn = new HttpClient())
                            {
                                cn.BaseAddress = new Uri(baseurl);
                                var content = JsonConvert.SerializeObject(aux);
                                var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                                var byteContent = new ByteArrayContent(buffer);
                                byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                                var postTask = cl.PutAsync("api/Usuarios/" + User.Usuario, byteContent).Result;

                                if (postTask.IsSuccessStatusCode)
                                {
                                    ModelState.AddModelError("ExitoContrasena", "La contraseña se ha cambiado exitosamente");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("ContrasenaNoCoinciden");
                        }

                    }
                    else
                    {
                        throw new Exception("ContrasenaIncorrecta");
                    }


                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "ContrasenaIncorrecta")
                {
                    ModelState.AddModelError("ErrorContrasena", "La contraseña anterior no es correcta");
                    return View();
                }
                else if (ex.Message == "ContrasenaNoCoinciden")
                {
                    ModelState.AddModelError("ErrorCoincidencia", "Las contraseñas digitadas no coinciden");
                    return View();
                }
            }

            return View();
        }

        public IActionResult RecuperarContrasena()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecuperarContrasena(data.Usuarios User)
        {
            try
            {

                data.Usuarios aux = new data.Usuarios();
                using (var cl = new HttpClient())
                {
                    cl.BaseAddress = new Uri(baseurl);
                    cl.DefaultRequestHeaders.Clear();
                    cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    User.ContrasenaAnterior = Seguridad.EncryptString(Key, User.ContrasenaAnterior);
                    User.Usuario = User.Usuario.ToLower();
                    HttpResponseMessage res = await cl.GetAsync("api/Usuarios/" + User.Usuario);

                    if (res.IsSuccessStatusCode)
                    {
                        var auxres = res.Content.ReadAsStringAsync().Result;
                        aux = JsonConvert.DeserializeObject<data.Usuarios>(auxres);

                        string cont = RetornarContrasena();

                        aux.Contrasena = Seguridad.EncryptString(Key, cont);
                        using (var cn = new HttpClient())
                        {
                            cn.BaseAddress = new Uri(baseurl);
                            var content = JsonConvert.SerializeObject(aux);
                            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                            var byteContent = new ByteArrayContent(buffer);
                            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                            var postTask = cl.PutAsync("api/Usuarios/" + User.Usuario, byteContent).Result;

                            if (postTask.IsSuccessStatusCode)
                            {
                                string body = CambiarCorreoCambiaContrasena(User.Usuario, cont);
                                Correo.EnviarCorreo(aux.Correo, "Cambio de contraseña", body);
                                ModelState.AddModelError("ExitoContrasena", "Se ha enviado una nueva contraseña a su correo");
                            }
                        }



                    }
                    else
                    {
                        throw new Exception("UsuarioNoExiste");
                    }


                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "UsuarioNoExiste")
                {
                    ModelState.AddModelError("ErrorUsuario", "El usuario digitado no existe");
                    return View();
                }
                else if (ex.Message == "ContrasenaNoCoinciden")
                {
                    ModelState.AddModelError("ErrorCoincidencia", "Las contraseñas digitadas no coinciden");
                    return View();
                }
            }

            return View();
            return View();
        }




        public IActionResult Registro()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
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

                            if (aux.Usuario != null)
                            {
                                var claims = new List<Claim>();

                                claims.Add(new Claim(ClaimTypes.Name, aux.Nombre));
                                if (User.Usuario == "admin")
                                {
                                    claims.Add(new Claim(ClaimTypes.Role, "Administrador"));
                                }
                                else if (aux.Tipo)
                                {

                                    claims.Add(new Claim(ClaimTypes.Role, "Empresa"));

                                    HttpContext.Session.SetInt32("CodEmpresa", (int)aux.CodEmpresa);
                                    HttpContext.Session.SetString("Usuario", aux.Usuario);

                                }
                                else
                                {
                                    HttpContext.Session.SetString("Usuario", aux.Usuario);

                                    claims.Add(new Claim(ClaimTypes.Role, "Usuario"));


                                }
                                var identity = new ClaimsIdentity(
        claims, CookieAuthenticationDefaults.
AuthenticationScheme);

                                var principal = new ClaimsPrincipal(identity);

                                var props = new AuthenticationProperties();


                                HttpContext.SignInAsync(
                                    CookieAuthenticationDefaults.
                        AuthenticationScheme,
                                    principal, props).Wait();

                                if (!aux.Tipo && User.Usuario != "admin")
                                {
                                    return RedirectToAction("PrincipalUsuario", "Home");
                                }
                                else
                                {
                                    return RedirectToAction("Index", "Home");
                                }



                            }
                        }

                        else if (res.StatusCode.ToString() == "NotFound")
                        {
                            ModelState.AddModelError("Error", "Usuario o contraseña son incorrectos");
                            return View();
                        }

                        else if (res.StatusCode.ToString() == "InternalServerError")
                        {
                            ModelState.AddModelError("ErrorServer", "Problema de conexión. Contacte al administrador");
                            return View();
                        }
                        else
                        {


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

        protected string CambiarCorreoCambiaContrasena(string Usuario, string Contrasena)
        {
            string Body = System.IO.File.ReadAllText("../FrontEnd.API/Tools/Plantillas_Correo/Correo_CambioContrasena.html");

            Body = Body.Replace("[Usuario]", Usuario);

            Body = Body.Replace("[Contrasena]", Contrasena);

            return Body;

        }


    }
}