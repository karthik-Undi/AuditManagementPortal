using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuditManagementPortal.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace AuditManagementPortal.Controllers
{
    public class LoginController : Controller
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(LoginController));
       
        //string token;
        static string token = "";

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            _log4net.Info("Login Page Was Called !!");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Userdetails Userdetails)
        {

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(Userdetails), Encoding.UTF8, "application/json");
                try
                {
                    using (var response = await httpClient.PostAsync("http://localhost:30781/api/Login/AuthenicateUser", content))
                    {

                        var Response = response.Content.ReadAsStringAsync().Result;
                        token = Response;
                        TempData["token"] = token;

                        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            _log4net.Info("Login Was Done With Email " + Userdetails.Email + " But the Credentials Were Wrong");
                            ViewBag.message = "Invalid User";
                        }

                        else
                        {
                            _log4net.Info("Login Was Done With Email " + Userdetails.Email + " And the Right Password");
                            ViewBag.message = "Success";
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Login API not Loaded. Please Try Later.";
                }
                return View();
            }
        }
    }
}