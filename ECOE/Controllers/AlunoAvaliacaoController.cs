using ECOE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECOE.Controllers
{
    public class AlunoAvaliacaoController : Controller
    {
        ECOEEntities bd = new ECOEEntities();
        
        [HttpPost]
        public bool AdicionarAlunoTurma(int TurmaId, int PessoaId)
        {
            var turma = new TurmaPessoa();
            turma.TurmaId = TurmaId;
            turma.PessoaId = PessoaId;
            turma.pessoaCadastrou = Convert.ToInt32(User.Identity.Name);
            turma.DataHora = DateTime.Now;
            bd.TurmaPessoa.Add(turma);
            bd.SaveChanges();
            return true;
        }

        [HttpPost]
        public bool AdicionarAlunoAvaliacao(int AvaliacaoId, int PessoaId)
        {
            var alunoAvaliacao = new AlunoAvaliacao();
            alunoAvaliacao.AvaliacaoId = AvaliacaoId;
            alunoAvaliacao.PessoaId = PessoaId;
            alunoAvaliacao.PessoaCadastrou = Convert.ToInt32(User.Identity.Name);
            alunoAvaliacao.DataCadastro = DateTime.Now;
            bd.AlunoAvaliacao.Add(alunoAvaliacao);
            bd.SaveChanges();
            return true;
        }
        [HttpPost]
        public bool AdicionarAlunoQuestao(int? radio, int? questao, int? aluno)
        {
            AlunoQuestao Aluno = new AlunoQuestao
            {
                DataHora = DateTime.Now,
                Nota = Convert.ToDouble(radio),
                QuestaoId = Convert.ToInt32(questao),
                PessoaId = Convert.ToInt32(aluno),
                AvaliadorId = Convert.ToInt32(HttpContext.User.Identity.Name)
            };
            bd.AlunoQuestao.Add(Aluno);
            bd.SaveChanges();
            return true;
        }

        [Authorize]
        public ActionResult AlunoExistente(int AvaliacaoId, string Mensagem, string Mensagem2)
        {
            ViewBag.Mensagem = Mensagem;
            var avaliacao = bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == AvaliacaoId);
            var turma = bd.Turma.FirstOrDefault(x => x.TurmaId == avaliacao.TurmaId);
            ViewBag.Mensagem2 = Mensagem2;
            ViewBag.turmaNome = turma.Nome;
            ViewBag.turma = turma.TurmaId;
            ViewBag.avaliacao = AvaliacaoId;
            ViewBag.dupla = avaliacao.Dupla;
            var alunos = bd.TurmaPessoa.Where(x => x.TurmaId == turma.TurmaId && x.Pessoa.AcessoId == 2);
            return View(alunos);
        }

        [HttpPost]
        public ActionResult AlunoExistente(string RA1, string RA2, int AvaliacaoId, int TurmaId)
        {
            var avaliador = Convert.ToInt32(HttpContext.User.Identity.Name);
            var avaliacao = bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == AvaliacaoId);
            var pessoa1 = bd.Pessoa.FirstOrDefault(x => x.RA == RA1);
            if (avaliacao.Dupla == false)
            {
                if (pessoa1 == null)
                    return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "2", Mensagem2 = "Aluno com o RA: " + RA1 + " Não foi cadastrado" });
                if (pessoa1.AcessoId != 2)
                    return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "4", Mensagem2 = "O RA: " + RA1 + " não pergtence a um aluno" });
                else
                {
                    if (bd.AlunoQuestao.Where(x => x.Questao.AvaliacaoId == AvaliacaoId && x.AvaliadorId == avaliador && x.PessoaId == pessoa1.PessoaId).ToList().Count() != 0)
                        return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });
                    if (bd.TurmaPessoa.FirstOrDefault(x => x.PessoaId == pessoa1.PessoaId && x.TurmaId == TurmaId) == null)
                        AdicionarAlunoTurma(TurmaId, pessoa1.PessoaId);
                    if (bd.AlunoAvaliacao.FirstOrDefault(x => x.PessoaId == pessoa1.PessoaId) == null)
                        AdicionarAlunoAvaliacao(AvaliacaoId, pessoa1.PessoaId);
                    return RedirectToAction("Avaliar", new { pessoa1.PessoaId, AvaliacaoId });
                }
            }
            else
            {
                var pessoa2 = bd.Pessoa.FirstOrDefault(x => x.RA == RA2);
                if (pessoa1 == null && pessoa2 == null)
                    return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "2", Mensagem2 = "Os alunos com RA's: " + RA1 + " e " + RA2 + " não foram cadastrados" });
                else if (pessoa1 == null)
                    return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "2", Mensagem2 = "Aluno com o RA: " + RA1 + " não foi cadastrado" });
                else if (pessoa2 == null)
                    return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "2", Mensagem2 = "Aluno com o RA: " + RA2 + " não foi cadastrado" });
                else if (pessoa1.AcessoId != 2 && pessoa2.AcessoId != 2)
                    return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "4", Mensagem2 = "Os RA's: " + RA1 + " e " + RA2 + " não pertencem a alunos" });
                else if (pessoa1.AcessoId != 2)
                    return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "4", Mensagem2 = "O RA: " + RA1 + " não pergtence a um aluno" });
                else if (pessoa2.AcessoId != 2)
                    return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "4", Mensagem2 = "O RA: " + RA2 + " não pergtence a um aluno" });
                else
                {
                    var alunoQuestao1 = bd.AlunoQuestao.Where(x => x.Questao.AvaliacaoId == AvaliacaoId && x.PessoaId == pessoa1.PessoaId).ToList().Count();
                    var alunoQuestao2 = bd.AlunoQuestao.Where(x => x.Questao.AvaliacaoId == AvaliacaoId && x.PessoaId == pessoa2.PessoaId).ToList().Count();

                    if (alunoQuestao1 > 0 && alunoQuestao2 > 0)
                        return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });
                    else if (alunoQuestao1 > 0)
                        return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });
                    else if (alunoQuestao2 > 0)
                        return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });
                    else
                    {
                        if (bd.TurmaPessoa.FirstOrDefault(x => x.PessoaId == pessoa1.PessoaId && x.TurmaId == TurmaId) == null)
                            AdicionarAlunoTurma(TurmaId, pessoa1.PessoaId);
                        if (bd.TurmaPessoa.FirstOrDefault(x => x.PessoaId == pessoa2.PessoaId && x.TurmaId == TurmaId) == null)
                            AdicionarAlunoTurma(TurmaId, pessoa2.PessoaId);
                        if (bd.AlunoAvaliacao.FirstOrDefault(x => x.PessoaId == pessoa1.PessoaId) == null)
                            AdicionarAlunoAvaliacao(AvaliacaoId, pessoa1.PessoaId);
                        if (bd.AlunoAvaliacao.FirstOrDefault(x => x.PessoaId == pessoa2.PessoaId) == null)
                            AdicionarAlunoAvaliacao(AvaliacaoId, pessoa2.PessoaId);
                        return RedirectToAction("Avaliar", new { pessoa1 = pessoa1.PessoaId, pessoa2 = pessoa2.PessoaId, AvaliacaoId });
                    }
                }
            }
        }

        [Authorize]
        public ActionResult NovoAluno(int? AvaliacaoId, int? RA1, string Mensagem)
        {
            ViewBag.Mensagem = Mensagem;
            var avaliacao = bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == AvaliacaoId).TurmaId;
            var turma = bd.Turma.FirstOrDefault(x => x.TurmaId == avaliacao);
            if (RA1 == null)
                ViewBag.ra = RA1;
            ViewBag.turmaNome = turma.Nome;
            ViewBag.turma = turma.TurmaId;
            ViewBag.avaliacao = AvaliacaoId;
            return View();
        }
        [HttpPost]
        public ActionResult NovoAluno(Pessoa pessoa, int AvaliacaoId, int TurmaId)
        {
            if (bd.Pessoa.FirstOrDefault(x => x.RA == pessoa.RA || x.Email == pessoa.Email) != null) return RedirectToAction("NovoAluno", new { AvaliacaoId, Mensagem = "2" });
            pessoa.AcessoId = 2;
            pessoa.StatusId = 1;
            pessoa.PessoaCadastrou = Convert.ToInt32(HttpContext.User.Identity.Name);
            bd.Pessoa.Add(pessoa);
            bd.SaveChanges();
            AdicionarAlunoTurma(TurmaId, pessoa.PessoaId);
            AdicionarAlunoAvaliacao(AvaliacaoId, pessoa.PessoaId);
            return RedirectToAction("Avaliar", new { pessoa.PessoaId, AvaliacaoId, Mensagem = "1" });
        }

        [Authorize]
        public ActionResult Avaliar(int? Pessoa1, int? Pessoa2, int AvaliacaoId, string Mensagem)
        {
            var avaliador = Convert.ToInt32(HttpContext.User.Identity.Name);
                       var alunoQuestao1 = bd.AlunoQuestao.Where(x => x.Questao.AvaliacaoId == AvaliacaoId && x.PessoaId == Pessoa1).ToList().Count();
            var alunoQuestao2 = bd.AlunoQuestao.Where(x => x.Questao.AvaliacaoId == AvaliacaoId && x.PessoaId == Pessoa2).ToList().Count();

            if (alunoQuestao1 > 0 && alunoQuestao2 > 0)
                return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });
            else if (alunoQuestao1 > 0)
                return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });
            else if (alunoQuestao2 > 0)
                return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });

            
            var questoes = bd.Questao.Where(x => x.AvaliacaoId == AvaliacaoId);
            var avaliado = bd.Pessoa.FirstOrDefault(x => x.PessoaId == Pessoa1).Nome;
            ViewBag.Mensagem = Mensagem;
            ViewBag.aluno1 = Pessoa1;

            if (Pessoa2 != null)
            {
                ViewBag.avaliado = "Alunos:" + avaliado + " e " + bd.Pessoa.FirstOrDefault(x => x.PessoaId == Pessoa2).Nome;
                ViewBag.aluno2 = Pessoa2;
            }
            else
                ViewBag.avaliado = "Aluno:" + avaliado;

            ViewBag.q = questoes.Select(x => x.Descricao);
            ViewBag.descricao = bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == AvaliacaoId).Descricao;
            ViewBag.ava = AvaliacaoId;
            ViewBag.cont = questoes.Count();
            return View(questoes);
        }

        [HttpPost]
        public ActionResult Avaliar(int AvaliacaoId, int Aluno1, int? Aluno2, string Radio, string Questao)
        {
            var radios = Radio.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var questoes = Questao.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var avaliador = Convert.ToInt32(HttpContext.User.Identity.Name);

            if (bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == AvaliacaoId).Questao.Count() != radios.Length)
            {

            }
            else
            {
                for (int i = 0; i < questoes.Length; i++)
                {
                    AdicionarAlunoQuestao(Convert.ToInt32(radios[i]), Convert.ToInt32(questoes[i]), Aluno1);
                    if (Aluno2 != null)
                        AdicionarAlunoQuestao(Convert.ToInt32(radios[i]), Convert.ToInt32(questoes[i]), Aluno2);
                }
            }

            if (Aluno2 != null)
                return RedirectToAction("ResultadoIndividual", new { Aluno1, Aluno2, AvaliacaoId });
            else
                return RedirectToAction("ResultadoIndividual", new { Aluno = Aluno1, AvaliacaoId });
        }

        [Authorize]
        public ActionResult ResultadoIndividual(int Aluno, int? Aluno2, int AvaliacaoId)
        {
            var resultado = bd.AlunoQuestao.Where(x => x.PessoaId == Aluno && x.Questao.AvaliacaoId == AvaliacaoId);
            ViewBag.Avaliacao = AvaliacaoId;
            ViewBag.nome = bd.Pessoa.FirstOrDefault(x => x.PessoaId == Aluno).Nome;
            var notam = bd.Questao.Where(x => x.AvaliacaoId == AvaliacaoId).Count() * 2;
            var nota = resultado.Select(x => x.Nota).Sum();
            ViewBag.nota = Math.Round(Convert.ToDouble((nota / notam) * Convert.ToDouble(bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == AvaliacaoId).Peso) / Convert.ToDouble(bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == AvaliacaoId).Peso)), 2);
            return View(resultado);
        }

        [Authorize]
        public ActionResult Resultado(int Aluno, int TurmaId)
        {

            var resultado = bd.AlunoQuestao.Where(x => x.PessoaId == Aluno && x.Questao.Avaliacoes.TurmaId == TurmaId);
            ViewBag.nome = bd.Pessoa.FirstOrDefault(x => x.PessoaId == Aluno).Nome;

            var avaliacoes = bd.Avaliacoes.Where(x => x.TurmaId == TurmaId).ToList();
            var questoes = bd.AlunoQuestao.Where(x => x.PessoaId == Aluno).ToList();
            var alunoQuestaoGeral = questoes.Join(avaliacoes,
                                      questao => questao.Questao.AvaliacaoId,
                                      avaliacao => avaliacao.AvaliacaoId,
                                      (questao, avaliacao) => questao
                                      );

            var notaMax = 0.0;
            var notaTotal = 0.0;
            var nota = 0.0;
            object[] notaAvaliacao = new object[avaliacoes.Count()];
            int cont = 0;
            foreach (var i in avaliacoes)
            {
                var avaliacao = avaliacoes.FirstOrDefault(x => x.AvaliacaoId == i.AvaliacaoId);
                var alunoQuestao = alunoQuestaoGeral.Where(x => x.Questao.AvaliacaoId == avaliacao.AvaliacaoId);
                notaMax = alunoQuestao.Count() * 2;
                if (notaMax > 0)
                {
                    notaTotal = alunoQuestao.Sum(x => x.Nota);
                    nota += (notaTotal / notaMax) * Convert.ToDouble(avaliacao.Peso);
                }
                notaAvaliacao[cont] = avaliacao.Nome + ": " + Math.Round((notaTotal / notaMax) * Convert.ToDouble(avaliacao.Peso), 2);
                cont++;
            }
            ViewBag.avaliacoesNotas = notaAvaliacao;
            ViewBag.nota = Math.Round(nota, 2);
            return View(resultado);
        }


    }
}