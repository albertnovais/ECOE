using ECOE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECOE.Controllers
{
    public class HomeController : Controller
    {
        ECOEEntities bd = new ECOEEntities();
        [Authorize]
        public ActionResult Index(Mensagem mensagem)
        {
            var usu = Convert.ToInt32(HttpContext.User.Identity.Name);
            var turmas = bd.TurmaPessoa.Where(x => x.PessoaId == usu).ToList();
            var alunoAvaliacao = bd.AlunoAvaliacao.Where(x => x.PessoaId == usu);
            var todasAvaliacoes = bd.Avaliacoes.Where(x => x.StatusId != 2 && x.DataAvaliacao >= DateTime.Now).ToList();
            ViewBag.Mensagem = mensagem;
            if (bd.Pessoa.FirstOrDefault(x => x.PessoaId == usu).AcessoId == 2)
            {
                var avali = todasAvaliacoes.Join(alunoAvaliacao,
                                        avalia => avalia.AvaliacaoId,
                                        aluno => aluno.AvaliacaoId,
                                        (avalia, aluno) => avalia);
                if (avali.Count() != 0)
                    ViewBag.mostrar = 1;
                else
                    ViewBag.mostrar = 0;
                return View(avali);
            }
            else
            {
                var avali = todasAvaliacoes.Join(turmas, avaliacao => avaliacao.TurmaId,
                                               turma => turma.TurmaId,
                                               (avaliacao, turma) => avaliacao
                                               );
                if (avali.Count() != 0)
                    ViewBag.mostrar = 1;
                else
                    ViewBag.mostrar = 0;
                return View(avali);
            }

        }

        public PartialViewResult Modal_Mensagem(Mensagem mensagem)
        {
            return PartialView(mensagem);
        }

        [Authorize]
        public ActionResult Sobre()
        {
            return View();
        }

        public JsonResult CursosPessoa()
        {
            teste teste = new teste();
            var cursos = bd.Curso.Where(x => x.StatusId == 1).ToList();
            var pessoasAvaliacao = bd.AlunoAvaliacao.ToList();
            List<int> q = new List<int>();
            List<string> n = new List<string>();
            foreach (var item in cursos)
            {
                q.Add(Convert.ToInt32(pessoasAvaliacao.Count(x => x.Avaliacoes.Turma.CursoId == item.CursoId)));
                n.Add(item.Nome);
            }
            teste.quantidades = q;
            teste.Nomes = n;
            return Json(teste, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AvaliacoesCurso()
        {
            teste teste = new teste();
            var cursos = bd.Curso.Where(x => x.StatusId == 1).ToList();
            var avaliacoes = bd.Avaliacoes.ToList();
            List<int> quantidadesAvaliacoes = new List<int>();
            List<string> n = new List<string>();
            foreach (var item in cursos)
            {
                quantidadesAvaliacoes.Add(avaliacoes.Count(x => x.Turma.Curso.CursoId == item.CursoId));
                n.Add(item.Nome);
            }
            teste.quantidades = quantidadesAvaliacoes;
            teste.Nomes = n;
            return Json(teste, JsonRequestBehavior.AllowGet);
        }
    }
}