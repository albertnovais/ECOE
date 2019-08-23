﻿using ECOE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ECOE.Controllers
{
    public class LoginController : Controller
    {
        ECOEEntities bd = new ECOEEntities();
        // GET: Login
        public ActionResult Login(int? mensagem)
        {
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (mensagem != null)
            {
                ViewBag.Mensagem = mensagem;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(Pessoa pessoa)
        {
            var u = bd.Pessoa.FirstOrDefault(x => x.Email == pessoa.Email && x.Senha == pessoa.Senha || x.RA == pessoa.Senha);
            if (u == null || pessoa.Senha == null)
            {
                return RedirectToAction("Login", "Login", new { mensagem = 2 });
            }
           
            FormsAuthentication.SetAuthCookie(u.PessoaId.ToString(), true);
            return RedirectToAction("Index", "Home");

        }

        public ActionResult LoginAluno(int? mensagem)
        {
            if (mensagem != null)
            {
                ViewBag.Mensagem = mensagem;
            }
            return View();
        }

        [HttpPost]
        public ActionResult LoginAluno(Pessoa pessoa)
        {
            var u = bd.Pessoa.FirstOrDefault(x => x.Email == pessoa.Email && x.RA == pessoa.RA);
            if (u == null)
            {
                return RedirectToAction("LoginAluno", "Login", new { mensagem = 2 });
            }
            FormsAuthentication.SetAuthCookie(u.PessoaId.ToString(), true);
            
            return RedirectToAction("Index", "Home");

        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }
    }
}