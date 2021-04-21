﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuditManagementPortal.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace AuditManagementPortal.Controllers
{
    public class AuditController : Controller
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AuditController));
        static List<QuestionsAndType> questionsAndTypes = new List<QuestionsAndType>();
        static Audit audit = new Audit();

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ChooseAuditType()
        {
            _log4net.Info("ChooseAuditType Was Called !!");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChooseAuditType(string AuditType)
        {
            _log4net.Info("ChooseAuditType Was Called !!");
            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync("http://localhost:19133/api/AuditCheckList/" + AuditType))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            _log4net.Info("AuditCheckList API with Audit type  " + AuditType + "was called");
                            ViewBag.message = "Success";
                            var Response = response.Content.ReadAsStringAsync().Result;
                            questionsAndTypes = JsonConvert.DeserializeObject<List<QuestionsAndType>>(Response);
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Login API not Loaded. Please Try Later.";
                }
                return RedirectToAction("ShowQuestions");
            }

        }
        public IActionResult ShowQuestions()
        {
            _log4net.Info("ChooseAuditType Was Called !!");
            return View(questionsAndTypes);
        }
        [HttpPost]
        public async Task<IActionResult> ShowQuestions(string projectname,string projectmanagername,string appownername,int radio1, int radio2, int radio3, int radio4, int radio5)
        {
            AuditDetails auditDetails = new AuditDetails();
            int nos = radio1 + radio2 + radio3 + radio4 + radio5;

            auditDetails.ApplicationOwnerName = appownername;
            auditDetails.CountOfNos = nos;
            auditDetails.ProjectManagerName = projectmanagername;
            auditDetails.ProjectName = projectname;
            auditDetails.AuditType = questionsAndTypes[0].AuditType;
            _log4net.Info("ChooseAuditType Was Called !!");
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(auditDetails), Encoding.UTF8, "application/json");
                try
                {
                    using (var response = await httpClient.PostAsync("http://localhost:34894/api/AuditSeverity",content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var Response = response.Content.ReadAsStringAsync().Result;
                            audit = JsonConvert.DeserializeObject<Audit>(Response);
                            _log4net.Info("Audit successfully submitted with audit ID "+auditDetails.Auditid);
                            ViewBag.message = "Success";
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Login API not Loaded. Please Try Later.";
                }
                return RedirectToAction("ShowProjectStatus");
            }
        }

        public IActionResult ShowProjectStatus()
        {
            _log4net.Info("ShowProjectStatus Was Called !!");
            return View(audit);
        }
    }
}