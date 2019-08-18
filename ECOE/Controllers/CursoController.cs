using ECOE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECOE.Controllers
{
    public class CursoController : Controller
    {
        ECOEEntities bd = new ECOEEntities();
        // GET: Curso
        [Authorize(Roles = "Adm, Coordenador")]
        public ActionResult ListCurso(int? Mensagem)
        {
            var lista = bd.Curso.Where(x => x.StatusId == 1).ToList();

            return View(lista);
        }

        [Authorize(Roles = "Adm")]
        public ActionResult CreateCurso()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCurso(Curso curso, string NomeU)
        {
            curso.PessoaId = Convert.ToInt32(HttpContext.User.Identity.Name);
            curso.StatusId = 1;
            bd.Curso.Add(curso);
            bd.SaveChanges();

            PessoaCurso(curso.CursoId, "");

            if (NomeU != "")
            {
                PessoaCurso(curso.CursoId, NomeU);
            }
            return RedirectToAction("ListCurso", "Curso");
        }
        public bool PessoaCurso(int CursoId, string NomeU)
        {
            int Pessoa;
            if (NomeU != "")
            {
                Pessoa = bd.Pessoa.FirstOrDefault(x => x.Nome == NomeU).PessoaId;
            }
            else
            {
                Pessoa = Convert.ToInt32(HttpContext.User.Identity.Name);
            }
            var pessoaCurso = new PessoaCurso
            {
                CursoId = CursoId,
                PessoaId = Pessoa,
                PessoaCadastrou = Convert.ToInt32(HttpContext.User.Identity.Name),
                Data = DateTime.Now.Date
            };
            bd.PessoaCurso.Add(pessoaCurso);
            bd.SaveChanges();
            return false;
        }

        [Authorize(Roles = "Adm")]
        public ActionResult EditCurso(int CursoId)
        {
            var curso = bd.Curso.FirstOrDefault(x => x.CursoId == CursoId);
            return View(curso);
        }

        [HttpPost]
        public ActionResult EditCurso(Curso curso, bool? remover)
        {
            var c = bd.Curso.FirstOrDefault(x => x.CursoId == curso.CursoId);
            if (remover == false)
            {
                c.Nome = curso.Nome;
                c.StatusId = curso.StatusId;
            }
            else c.StatusId = 2;
            c.PessoaId = Convert.ToInt32(HttpContext.User.Identity.Name);
            bd.Entry(c).State = System.Data.Entity.EntityState.Modified;
            bd.SaveChanges();
            return RedirectToAction("ListCurso", "Curso", new { Mensagem = 1 });
        }

        [Authorize(Roles = "Adm")]
        public ActionResult CoordenadorCurso(int cursoId)
        {
            ViewBag.cursoNome = bd.Curso.FirstOrDefault(x => x.CursoId == cursoId).Nome;
            ViewBag.cursoId = cursoId;
            return View();
        }

        [HttpPost]
        public ActionResult CoordenadorCurso(PessoaCurso pessoaCurso, string Nome)
        {
            PessoaCurso(pessoaCurso.CursoId, Nome);          
            return RedirectToAction("ListCurso");
        }

    }
}