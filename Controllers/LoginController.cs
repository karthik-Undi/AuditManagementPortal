using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuditManagementPortal.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;

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
                            
                            //Validate Google recaptcha here
                            var captcharesponse = Request.Form["g-recaptcha-response"];
                            string secretKey = "6Lcs0LIaAAAAAFYxmFi4KziD-m744DcpRQFZzKqj";
                            var client = new WebClient();
                            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, captcharesponse));
                            var obj = JObject.Parse(result);
                            var status = (bool)obj.SelectToken("success");
                            ViewBag.Message = status ? "Google reCaptcha validation success" : "Google reCaptcha validation failed";
                            if (status == false)
                            {
                                ViewBag.Message = "Please verify reCaptcha";
                                return View();
                            }

                            return RedirectToAction("ChooseAuditType","Audit");
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