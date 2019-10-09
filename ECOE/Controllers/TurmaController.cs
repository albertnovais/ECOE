using ECOE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECOE.Controllers
{
    public class TurmaController : Controller
    {
        ECOEEntities bd = new ECOEEntities();

        public bool AdicionarPessoaTurma(TurmaPessoa turmaPessoa)
        {
            turmaPessoa.pessoaCadastrou = Convert.ToInt32(HttpContext.User.Identity.Name);
            turmaPessoa.DataHora = DateTime.Now.Date;
            bd.TurmaPessoa.Add(turmaPessoa);
            bd.SaveChanges();
            return true;
        }

        [Authorize(Roles = "Adm , Coordenador, Avaliador")]
        public ActionResult ListTurma()
        {
            var turmas = bd.Turma.Where(x => x.StatusId == 1).ToList();
            return View(turmas);
        }

        [Authorize(Roles = "Adm , Coordenador")]
        public ActionResult CreateTurma()
        {
            var usu = Convert.ToInt32(HttpContext.User.Identity.Name);
            var curso = bd.Curso.Where(x => x.StatusId == 1).ToList();
            var pessoa = bd.PessoaCurso.ToList();
            var cursos = from Curso in curso
                         join Pessoa in pessoa on Curso.CursoId equals Pessoa.CursoId
                         where Pessoa.PessoaId == usu
                         select new { Curso.Nome, Curso.CursoId };

            ViewBag.curso = new SelectList(cursos, "CursoId", "Nome");
            return View();
        }

        [HttpPost]
        public ActionResult CreateTurma(Turma turma)
        {
            var nome = bd.Curso.FirstOrDefault(x => x.CursoId == turma.CursoId).Nome;

            turma.Ano = DateTime.Now.Year;
            turma.Nome = turma.Periodo + "º " + turma.Turno + " " + nome + " " + turma.Ano;
            turma.PessoaId = Convert.ToInt32(HttpContext.User.Identity.Name);
            turma.StatusId = 1;
            bd.Turma.Add(turma);
            bd.SaveChanges();

            var turmaPessoa = new TurmaPessoa
            {
                PessoaId = Convert.ToInt32(HttpContext.User.Identity.Name),
                TurmaId = turma.TurmaId
            };
            AdicionarPessoaTurma(turmaPessoa);

            return RedirectToAction("ListTurma", "Turma");
        }

        [Authorize(Roles = "Adm")]
        public ActionResult EditTurma(int turmaId)
        {
            var turma = bd.Turma.FirstOrDefault(x => x.TurmaId == turmaId);
            return View(turma);
        }

        [HttpPost]
        public ActionResult EditTurma(Turma turma, bool? remover)
        {
            var tur = bd.Turma.FirstOrDefault(x => x.TurmaId == turma.TurmaId);
            if (remover == false)
            {
                tur.Ano = turma.Ano;
                tur.Nome = turma.Nome;
            }
            else
            {
                tur.StatusId = 2;
            }
            bd.Entry(tur).State = System.Data.Entity.EntityState.Modified;
            bd.SaveChanges();
            return RedirectToAction("ListTurma", "Turma", new { Mensagem = 1 });
        }

        //[HttpPost]
        //public ActionResult RemoveTurma(int id)
        //{
        //    var turma = bd.Turma.FirstOrDefault(x => x.TurmaId == id);
        //    turma.StatusId = 2;

        //    return RedirectToAction("ListTurma", "Turma", new { Mensagem = 1 });
        //}

        [Authorize(Roles = "Adm , Coordenador")]
        public ActionResult AvaliadorTurma(int TurmaId, string Mensagem)
        {
            ViewBag.Mensagem = Mensagem;
            ViewBag.turma = TurmaId;
            ViewBag.turman = bd.Turma.FirstOrDefault(x => x.TurmaId == TurmaId).Nome;
            var avaliadores = bd.TurmaPessoa.Where(x => x.TurmaId == TurmaId && x.Pessoa.AcessoId != 2);
            return View(avaliadores);
        }

        [HttpPost]
        public ActionResult AvaliadorTurma(TurmaPessoa turmaPessoa, string Nome)
        {
            var pessoa = bd.Pessoa.FirstOrDefault(x => x.Nome == Nome);
            if (pessoa == null)
            {
                return RedirectToAction("CreatePessoa", "Pessoa", new { turmaPessoa.TurmaId, Mensagem = "3" });
            }
            if (bd.TurmaPessoa.FirstOrDefault(x => x.PessoaId == pessoa.PessoaId && x.TurmaId == turmaPessoa.TurmaId) == null)
            {
                if (pessoa.StatusId != 2)
                {
                    turmaPessoa.PessoaId = pessoa.PessoaId;
                    AdicionarPessoaTurma(turmaPessoa);
                    return RedirectToAction("AvaliadorTurma", new { turmaPessoa.TurmaId, Mensagem = "1" });
                }
                else
                {
                    return RedirectToAction("AvaliadorTurma", new { turmaPessoa.TurmaId, Mensagem = "3" });
                }
            }
            else
            {
                return RedirectToAction("AvaliadorTurma", new { turmaPessoa.TurmaId, Mensagem = "2" });
            }
        }

        [Authorize(Roles = "Adm , Coordenador, Avaliador")]
        public ActionResult AlunoTurma(int TurmaId, string Mensagem)
        {
            ViewBag.Mensagem = Mensagem;
            ViewBag.turma = TurmaId;
            ViewBag.turman = bd.Turma.FirstOrDefault(x => x.TurmaId == TurmaId).Nome;
            var alunos = bd.TurmaPessoa.Where(x => x.TurmaId == TurmaId && x.Pessoa.AcessoId == 2).OrderBy(x=>x.Pessoa.Nome);
            var quantidadeAlunos = alunos.Count();
            //var avaliacoes = bd.Avaliacoes.Where(x => x.TurmaId == TurmaId).ToList();
            //var questoes = bd.AlunoQuestao.ToList();
            //var alunoQuestaoGeral = questoes.Join(avaliacoes,
            //                          questao => questao.Questao.AvaliacaoId,
            //                          avaliacao => avaliacao.AvaliacaoId,
            //                          (questao, avaliacao) => questao
            //                          );

            //var notaMax = 0.0;
            //var notaTotal = 0.0;
            //var nota = 0.0    ;
            //object [] notaAvaliacao = new object [avaliacoes.Count()];
            //int cont = 0;
            //foreach (var i in avaliacoes)
            //{
            //    var avaliacao = avaliacoes.FirstOrDefault(x => x.AvaliacaoId == i.AvaliacaoId);
            //    var alunoQuestao = alunoQuestaoGeral.Where(x => x.Questao.AvaliacaoId == avaliacao.AvaliacaoId);
            //    notaMax += alunoQuestao.Count() * 2;
            //    notaTotal += alunoQuestao.Sum(x => x.Nota);
            //    nota +=(notaTotal / notaMax) * Convert.ToDouble(avaliacao.Peso);
            //    notaAvaliacao[cont] = avaliacao.Nome + ": " + Math.Round((notaTotal / notaMax) * Convert.ToDouble(avaliacao.Peso), 2);
            //    cont++;
            //}
            //ViewBag.avaliacoesNotas = notaAvaliacao;
            //ViewBag.nota = Math.Round(nota, 2);
            return View(alunos);
        }

        [HttpPost]
        public ActionResult AlunoTurma(TurmaPessoa turmaPessoa, string RA)
        {
            var usu = bd.Pessoa.FirstOrDefault(x => x.RA == RA);
            if (usu != null)
            {
                if (bd.TurmaPessoa.FirstOrDefault(x => x.PessoaId == usu.PessoaId && x.TurmaId == turmaPessoa.TurmaId) == null)
                {
                    if (usu.AcessoId == 2)
                    {
                        turmaPessoa.PessoaId = usu.PessoaId;
                        AdicionarPessoaTurma(turmaPessoa);
                        return RedirectToAction("AlunoTurma", new { turmaPessoa.TurmaId, Mensagem = "1" });
                    }
                    else
                    {
                        return RedirectToAction("AlunoTurma", new { turmaPessoa.TurmaId, Mensagem = "3" });
                    }
                }
                else
                {
                    return RedirectToAction("AlunoTurma", new { turmaPessoa.TurmaId, Mensagem = "2" });
                }

            }
            else
            {
                return RedirectToAction("NovoAluno", new { turmaPessoa.TurmaId, RA, Mensagem = "1" });
            }

        }

        [Authorize]
        public ActionResult NovoAluno(int? TurmaId, int? RA, string Mensagem)
        {
            ViewBag.Mensagem = Mensagem;

            var turma = bd.Turma.FirstOrDefault(x => x.TurmaId == TurmaId);
            if (RA == null)
                ViewBag.ra = RA;
            ViewBag.turmaNome = turma.Nome;
            ViewBag.turma = turma.TurmaId;
            return View();
        }
        [HttpPost]
        public ActionResult NovoAluno(Pessoa pessoa, int AvaliacaoId, int TurmaId)
        {

            if (bd.Pessoa.FirstOrDefault(x => x.RA == pessoa.RA) != null) return RedirectToAction("NovoAluno", new { AvaliacaoId, Mensagem = "2" });
            pessoa.StatusId = 1;
            pessoa.AcessoId = 2;
            pessoa.PessoaCadastrou = Convert.ToInt32(User.Identity.Name);
            bd.Pessoa.Add(pessoa);
            bd.SaveChanges();
            var turmaPessoa = new TurmaPessoa();
            turmaPessoa.TurmaId = TurmaId;
            turmaPessoa.PessoaId = pessoa.PessoaId;
            AdicionarPessoaTurma(turmaPessoa);
            return RedirectToAction("AlunoTurma", new { TurmaId, Mensagem = "1" });
        }

    }
}