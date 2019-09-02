using ECOE.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ECOE.Controllers
{
    public class AvaliacaoController : Controller
    {
        ECOEEntities bd = new ECOEEntities();
        // GET: Avaliacao
        [Authorize(Roles = "Adm , Coordenador, Avaliador")]
        public ActionResult ListAvaliacao(int? Mensagem)
        {
            var usu = Convert.ToInt32(HttpContext.User.Identity.Name);
            var turmas = bd.TurmaPessoa.Where(x => x.PessoaId == usu).ToList();
            var avaliacoes = bd.Avaliacoes.Where(x => x.StatusId != 2).ToList();
            var avali = avaliacoes.Join(turmas,
                                            avaliacao => avaliacao.TurmaId,
                                            turma => turma.TurmaId,
                                            (avaliacao, turma) => avaliacao
                                            );
            return View(avali);
        }

        [Authorize(Roles = "Adm , Coordenador")]
        public ActionResult CreateAvaliacao()
        {
            var usu = Convert.ToInt32(HttpContext.User.Identity.Name);
            var curso = bd.Curso.ToList();
            var pessoa = bd.PessoaCurso.ToList();
            var turma = bd.Turma.Where(x=> x.StatusId ==1).ToList();
            var cursos = from Curso in curso
                         join Pessoa in pessoa on Curso.CursoId equals Pessoa.CursoId
                         where Pessoa.PessoaId == usu
                         select new { Curso.Nome, Curso.CursoId };
            var turmas = from Turma in turma
                         join Curso in cursos on Turma.CursoId equals Curso.CursoId
                         select new { Turma.Nome, Turma.TurmaId };
            ViewBag.turma = new SelectList(turmas, "TurmaId", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateAvaliacao(Avaliacoes avaliacoes)
        {
            
            avaliacoes.PessoaId = Convert.ToInt32(HttpContext.User.Identity.Name);
            avaliacoes.DataCadastro = DateTime.Now.Date;
            avaliacoes.StatusId = 1;
            bd.Avaliacoes.Add(avaliacoes);
            bd.SaveChanges();
            return RedirectToAction("ListAvaliacao", "Avaliacao");
        }

        [Authorize(Roles = "Adm, Coordenador")]
        public ActionResult EditAvaliacao(int avaliacaoId)
        {
            var avaliacao = bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == avaliacaoId);
            return View(avaliacao);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditAvaliacao(Avaliacoes avaliacao)
        {
            var p = bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == avaliacao.AvaliacaoId);            
            p.Nome = avaliacao.Nome;
            p.Descricao = avaliacao.Descricao;
            p.Peso = avaliacao.Peso;
            bd.Entry(p).State = EntityState.Modified;
            bd.SaveChanges();
            return RedirectToAction("ListAvaliacao", "Avaliacao", new { Mensagem = 1 });
        }

        [HttpPost]
        public ActionResult RemoveAvaliacao(int id)
        {
            var p = bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == id);
            p.StatusId = 2;
            p.PessoaId = Convert.ToInt32(HttpContext.User.Identity.Name);
            bd.Entry(p).State = EntityState.Modified;
            bd.SaveChanges();
            return RedirectToAction("ListAvaliacao", "Avaliacao", new { Mensagem = 1 });
        }

        //[Authorize(Roles = "Adm , Coordenador")]
        //public ActionResult CreateGrupo(int AvaliacaoId)
        //{
        //    ViewBag.ava = AvaliacaoId;
        //    ViewBag.avan = bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == AvaliacaoId).Nome;
        //    return View();
        //}
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult CreateGrupo(GrupoQuestao grupoQuestao, string sair)
        //{
        //    if (sair != null)
        //    {
        //        return RedirectToAction("ListAvaliacao", "Avaliacao");
        //    }
           
        //    grupoQuestao.StatusId = 2;
        //    grupoQuestao.PessoaId = Convert.ToInt32(HttpContext.User.Identity.Name);
        //    bd.GrupoQuestao.Add(grupoQuestao);
        //    bd.SaveChanges();

        //    return RedirectToAction("CreateQuestao", "Avaliacao", new { GrupoId = grupoQuestao.GrupoId });
        //}

        [Authorize(Roles = "Adm , Coordenador")]
        public ActionResult CreateQuestao(int AvaliacaoId)
        {
            ViewBag.avaliacaoID = AvaliacaoId;
            //ViewBag.grupoNome = bd.GrupoQuestao.FirstOrDefault(x => x.GrupoId == GrupoId).Nome;
            ViewBag.Nome = bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == AvaliacaoId).Nome;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateQuestao(Questao questao, string sair)
        {
            if (sair != null)
            {
                return RedirectToAction("ListAvaliacao", "Avaliacao");
            }
            questao.DataCadastro = DateTime.Now.Date;
            questao.StatusId = 1;
            questao.PessoaId = Convert.ToInt32(HttpContext.User.Identity.Name);
            bd.Questao.Add(questao);
            bd.SaveChanges();            
            //return RedirectToAction("CreateQuestao", "Avaliacao", new { GrupoId = questao.GrupoId });
            return RedirectToAction("CreateQuestao", "Avaliacao", new { avaliacaoId= questao.AvaliacaoId });
        }

        [Authorize(Roles = "Adm, Coordenador")]
        public ActionResult EditQuestao(int questaoId)
        {
            var questao = bd.Questao.FirstOrDefault(x => x.QuestaoId == questaoId);
            return View(questao);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditQuestao(Questao questao)
        {
            var p = bd.Questao.FirstOrDefault(x => x.QuestaoId == questao.QuestaoId);            
            p.Descricao = questao.Descricao;            
            bd.Entry(p).State = EntityState.Modified;
            bd.SaveChanges();
            return RedirectToAction("ListQuetao", "Avaliacao", new { Mensagem = 1 });
        }

        [HttpPost]
        public ActionResult RemoveQuestao(int id)
        {
            var p = bd.Questao.FirstOrDefault(x => x.QuestaoId == id);
            p.StatusId = 2;
            p.PessoaId = Convert.ToInt32(HttpContext.User.Identity.Name);
            bd.Entry(p).State = EntityState.Modified;
            bd.SaveChanges();
            return RedirectToAction("ListAvaliacao", "Avaliacao", new { Mensagem = 1 });
        }

        //[Authorize(Roles = "Adm , Coordenador")]
        //public ActionResult ListGrupo(int avaliacaoId)
        //{
        //    var list = bd.GrupoQuestao.Where(x => x.AvaliacaoId == avaliacaoId /*&& x.StatusId == 1*/).ToList();
        //    var avaliacao = bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == avaliacaoId);
        //    ViewBag.avaliacaoId = avaliacao.AvaliacaoId;
        //    ViewBag.avaliacao = avaliacao.Nome;
        //    return View(list);
        //}

        [Authorize(Roles = "Adm , Coordenador")]
        public ActionResult ListQuestao(int AvaliacaoId)
        {
            var list = bd.Questao.Where(x => x.AvaliacaoId == AvaliacaoId /*&& x.StatusId == 1*/).ToList();
            ViewBag.avaliacao = AvaliacaoId;

            return View(list);
        }
    }
}