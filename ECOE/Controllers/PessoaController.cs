using ECOE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECOE.Controllers
{
    public class PessoaController : Controller
    {
        ECOEEntities bd = new ECOEEntities();

        //Cria Pessoa
        public bool CriarPessoa(Pessoa pessoa)
        {
            if (bd.Pessoa.FirstOrDefault(x => x.Email == pessoa.Email || x.RA == pessoa.RA) != null)
                return false;
            pessoa.StatusId = 1;
            bd.Pessoa.Add(pessoa);
            bd.SaveChanges();
            return true;
        }
        public bool EditarPessoa(Pessoa pessoa)
        {
            var p = bd.Pessoa.FirstOrDefault(x => x.PessoaId == pessoa.PessoaId);
            if (p == null) return false;
            p.Nome = pessoa.Nome;
            p.Email = pessoa.Email;
            p.StatusId = pessoa.StatusId;
            p.AcessoId = pessoa.AcessoId;
            p.PessoaCadastrou = Convert.ToInt32(HttpContext.User.Identity.Name);
            bd.Entry(p).State = System.Data.Entity.EntityState.Modified;
            bd.SaveChanges();
            return true;
        }


        [Authorize(Roles = "Adm , Coordenador")]
        public ActionResult ListaPessoa(Mensagem Mensagem, bool? alunos)
        {
            ViewBag.Mensagem = Mensagem;
            var l = Convert.ToInt32(HttpContext.User.Identity.Name);
            var logado = bd.Pessoa.FirstOrDefault(x => x.PessoaId == l).AcessoId;
            List<Pessoa> lista;
            if (alunos == true)
            {
                lista = bd.Pessoa.Where(x => x.AcessoId == 2 && x.StatusId == 1).ToList();
            }
            else
            {
                if (logado == 4)
                {
                    lista = bd.Pessoa.Where(x => x.AcessoId != 1 || x.AcessoId != 2 && x.StatusId == 1).OrderBy(x => x.Nome).ToList();
                }
                else if (logado == 1)
                {
                    lista = bd.Pessoa.Where(x => x.AcessoId != 2).OrderBy(x => x.StatusId).ToList();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }


            return View(lista);
        }

        [Authorize(Roles = "Adm , Coordenador")]
        public ActionResult CreatePessoa(string Mensagem)
        {
            ViewBag.Mensagem = Mensagem;
            return View();
        }

        [HttpPost]
        public ActionResult CreatePessoa(Pessoa pessoa)
        {
            if (bd.Pessoa.FirstOrDefault(x => x.Email == pessoa.Email) != null) return RedirectToAction("CreatePessoa", "Pessoa", new { texto = "Email já cadastrado", tipo = 2 });
            pessoa.PessoaCadastrou = Convert.ToInt32(HttpContext.User.Identity.Name);
            if (CriarPessoa(pessoa))
            {
                return RedirectToAction("ListaPessoa", "Pessoa", new { texto = "Pessoa criada com sucesso", tipo = 1 });
            }
            else
            {
                return RedirectToAction("CreatePessoa", "Pessoa", new { texto = "Opa algo deu errado", tipo = 2 });
            }

            
        }

        [Authorize(Roles = "Adm , Coordenador")]
        public ActionResult EditPessoa(int pessoaId)
        {
            var pessoa = bd.Pessoa.FirstOrDefault(x => x.PessoaId == pessoaId);
            return View(pessoa);
        }


        [HttpPost]
        public ActionResult EditPessoa(Pessoa pessoa)
        {
            EditarPessoa(pessoa);
            return RedirectToAction("ListaPessoa", "Pessoa", new { Mensagem = "1" });
        }
        [Authorize(Roles = "Adm , Coordenador, Avaliador")]
        public PartialViewResult Modal_Editar_RA()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Modal_Editar_RA(Pessoa pessoa)
        {
            var p = bd.Pessoa.FirstOrDefault(x => x.PessoaId == pessoa.PessoaId);
            p.RA = pessoa.RA;
            p.PessoaCadastrou = Convert.ToInt32(HttpContext.User.Identity.Name);
            bd.Entry(p).State = System.Data.Entity.EntityState.Modified;
            bd.SaveChanges();

            var texto = "Edição Efetuada com Sucesso";
            var tipo = 1;
           
            return RedirectToAction("ListaPessoa", "Pessoa", new {  texto, tipo });
        }

        [Authorize(Roles = "Adm , Coordenador, Avaliador")]
        public ActionResult TrocarSenha(int pessoaId, string Mensagem)
        {
            var pessoa = bd.Pessoa.FirstOrDefault(x => x.PessoaId == pessoaId);
            return View(pessoa);
        }

        [HttpPost]
        public ActionResult TrocarSenha(Pessoa pessoa, string senhaAntiga)
        {
            var p = bd.Pessoa.FirstOrDefault(x => x.PessoaId == pessoa.PessoaId);
            if (p.Senha == senhaAntiga)
            {
                p.Senha = pessoa.Senha;
                bd.Entry(p).State = System.Data.Entity.EntityState.Modified;
                bd.SaveChanges();
                return RedirectToAction("Index", "Home", new { Mensagem = "1" });
            }
            else
            {
                return RedirectToAction("TrocarSenha", new { pessoa.PessoaId, Mensagem = "2" });
            }
        }

        [HttpPost]
        public ActionResult RemovePessoa(int id)
        {
            var p = bd.Pessoa.FirstOrDefault(x => x.PessoaId == id);
            p.StatusId = 2;
            p.PessoaCadastrou = Convert.ToInt32(HttpContext.User.Identity.Name);
            bd.Entry(p).State = System.Data.Entity.EntityState.Modified;
            bd.SaveChanges();
            return RedirectToAction("ListaPessoa", "Pessoa", new { Mensagem = "1" });
        }

        public ActionResult CreateAluno(int CoordId, int? Mensagem)
        {
            ViewBag.Coord = CoordId;
            ViewBag.Mensagem = Mensagem;
            return View();
        }

        [HttpPost]
        public ActionResult CreateAluno(Pessoa pessoa)
        {
            if (bd.Pessoa.FirstOrDefault(x => x.RA == pessoa.RA || x.Email == pessoa.Email) != null) return RedirectToAction("CreateAluno", "Pessoa", new { CoordId = pessoa.PessoaCadastrou, Mensagem = 2 });
            pessoa.AcessoId = 2;
            pessoa.PessoaCadastrou = pessoa.PessoaCadastrou;
            CriarPessoa(pessoa);
            return RedirectToAction("CreateAluno", "Pessoa", new { CoordId = pessoa.PessoaCadastrou, Mensagem = 1 });
        }

        public ActionResult Search(string term)
        {
            return Json(bd.Pessoa.Where(c => c.Nome.StartsWith(term) && c.AcessoId != 2).Select(a => new { label = a.Nome, id = a.PessoaId }), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchAluno(string term)
        {
            return Json(bd.Pessoa.Where(c => c.Nome.StartsWith(term) && c.AcessoId == 2).Select(a => new { label = a.Nome, id = a.PessoaId, ra = a.RA }), JsonRequestBehavior.AllowGet);
        }

    }
}