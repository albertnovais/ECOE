using ECOE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECOE.Controllers
{
    public class HomeController : Controller
    {
        ECOEEntities bd = new ECOEEntities();
        [Authorize]
        public ActionResult Index()
        {
            var usu = Convert.ToInt32(HttpContext.User.Identity.Name);
            var turmas = bd.TurmaPessoa.Where(x => x.PessoaId == usu).ToList();
            var alunoAvaliacao = bd.AlunoAvaliacao.Where(x => x.PessoaId == usu);
            var todasAvaliacoes = bd.Avaliacoes.Where(x => x.StatusId != 2).ToList();

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

    }
}