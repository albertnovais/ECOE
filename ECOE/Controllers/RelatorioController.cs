﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECOE.Models;

namespace ECOE.Controllers
{
    public class RelatorioController : Controller
    {
        ECOEEntities bd = new ECOEEntities();
        // GET: Relatorio
        public ActionResult Index(Mensagem Mensagem)
        {
            ViewBag.Mensagem = Mensagem;
            return View();
        }
    }
}