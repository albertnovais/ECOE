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
        //[Authorize]
        //public ActionResult Escolha(int AvaliacaoId)
        //{
        //    ViewBag.avaliacao = AvaliacaoId;
        //    return View();
        //}
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

        [Authorize]
        public ActionResult AlunoExistente(int AvaliacaoId, string Mensagem)
        {
            ViewBag.Mensagem = Mensagem;
            var avaliacao = bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == AvaliacaoId);
            var turma = bd.Turma.FirstOrDefault(x => x.TurmaId == avaliacao.TurmaId);
            ViewBag.turmaNome = turma.Nome;
            ViewBag.turma = turma.TurmaId;
            ViewBag.avaliacao = AvaliacaoId;
            //ViewBag.dupla = avaliacao.dupla;
            var alunos = bd.TurmaPessoa.Where(x => x.TurmaId == turma.TurmaId && x.Pessoa.AcessoId == 2);
            return View(alunos);

        }
        [HttpPost]
        public ActionResult AlunoExistente(string RA1, string RA2, int AvaliacaoId, int TurmaId)
        {
            var avaliador = Convert.ToInt32(HttpContext.User.Identity.Name);
            //var avaliacao = bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == AvaliacaoId);
            //if (avaliacao.dupla == true)
            //{
            var pessoa = bd.Pessoa.FirstOrDefault(x => x.RA == RA1);
            if (pessoa == null)
            {
                string Mensagem = "1";
                return RedirectToAction("NovoAluno", new { AvaliacaoId, RA1, Mensagem });
            }
            else if (pessoa.AcessoId != 2)
            {
                return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "4" });
            }
            else
            {
                if (bd.TurmaPessoa.FirstOrDefault(x => x.PessoaId == pessoa.PessoaId && x.TurmaId == TurmaId) == null)
                {
                    AdicionarAlunoTurma(TurmaId, pessoa.PessoaId);
                }
                else
                {
                    if (bd.AlunoQuestao.Where(x => x.Questao.AvaliacaoId == AvaliacaoId && x.AvaliadorId == avaliador && x.PessoaId == pessoa.PessoaId).ToList().Count() != 0)
                    {
                        return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });
                    }
                    return RedirectToAction("Avaliar", new { pessoa.PessoaId, AvaliacaoId });
                }
               
                AdicionarAlunoAvaliacao(AvaliacaoId, pessoa.PessoaId);
                return RedirectToAction("Avaliar", new { pessoa, AvaliacaoId });
            }

            //else
            //{
            //    var pessoa1 = bd.Pessoa.FirstOrDefault(x => x.RA == RA1);
            //    var pessoa2 = bd.Pessoa.FirstOrDefault(x => x.RA == RA2);
            //    if (pessoa1 == null)
            //    {
            //        string Mensagem = "1";
            //        return RedirectToAction("NovoAluno", new { AvaliacaoId, RA1, Mensagem });
            //    }
            //    else if (pessoa.AcessoId != 2)
            //    {
            //        return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "4" });
            //    }
            //    else
            //    {
            //        if (bd.TurmaPessoa.FirstOrDefault(x => x.PessoaId == pessoa.PessoaId && x.TurmaId == TurmaId) == null)
            //        {
            //            AdicionarAlunoTurma(TurmaId, pessoa.PessoaId);
            //        }
            //        else
            //        {
            //            if (bd.AlunoQuestao.Where(x => x.Questao.AvaliacaoId == AvaliacaoId && x.PessoaId == pessoa.PessoaId).ToList().Count() > 0)
            //            {
            //                return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });
            //            }
            //            return RedirectToAction("Avaliar", new { pessoa.PessoaId, AvaliacaoId });
            //        }
            //        AdicionarAlunoAvaliacao(AvaliacaoId, pessoa.PessoaId);
            //        return RedirectToAction("Avaliar", new { pessoa, AvaliacaoId });
            //    }
            //}

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
        public ActionResult avaliar(int PessoaId, int AvaliacaoId, string Mensagem)
        {
            var avaliador = Convert.ToInt32(HttpContext.User.Identity.Name);
            if (bd.AlunoQuestao.Where(x => x.Questao.AvaliacaoId == AvaliacaoId && x.AvaliadorId == avaliador && x.PessoaId== PessoaId).ToList().Count() != 0)
            {
                return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });
            }
            //if (bd.AlunoQuestao.Where(x => x.PessoaId == PessoaId && x.Questao.AvaliacaoId == AvaliacaoId).Count() != 0) return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });
            ViewBag.Mensagem = Mensagem;
            var questoes = bd.Questao.Where(x => x.AvaliacaoId == AvaliacaoId);
            ViewBag.avaliado = bd.Pessoa.FirstOrDefault(x => x.PessoaId == PessoaId).Nome;
            ViewBag.q = questoes.Select(x => x.Descricao);
            ViewBag.descricao = bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == AvaliacaoId).Descricao;
            ViewBag.aluno = PessoaId;
            ViewBag.ava = AvaliacaoId;
            ViewBag.cont = questoes.Count();
            return View(questoes);
        }

        [HttpPost]
        public ActionResult avaliar(int AvaliacaoId, int Aluno,
            int? QuestaoId1, int? QuestaoId2, int? QuestaoId3, int? QuestaoId4, int? QuestaoId5, int? QuestaoId6, int? QuestaoId7, int? QuestaoId8, int? QuestaoId9, int? QuestaoId10,
            int? QuestaoId11, int? QuestaoId12, int? QuestaoId13, int? QuestaoId14, int? QuestaoId15, int? QuestaoId16, int? QuestaoId17, int? QuestaoId18, int? QuestaoId19, int? QuestaoId20,
            int? Radio1, int? Radio2, int? Radio3, int? Radio4, int? Radio5, int? Radio6, int? Radio7, int? Radio8, int? Radio9, int? Radio10,
            int? Radio11, int? Radio12, int? Radio13, int? Radio14, int? Radio15, int? Radio16, int? Radio17, int? Radio18, int? Radio19, int? Radio20)
        {
            var avaliador = Convert.ToInt32(HttpContext.User.Identity.Name);
            if (bd.AlunoQuestao.FirstOrDefault(x => x.QuestaoId == QuestaoId1 && x.PessoaId == Aluno) != null) return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });
            if (Radio1 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio1),
                    QuestaoId = Convert.ToInt32(QuestaoId1),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio2 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio2),
                    QuestaoId = Convert.ToInt32(QuestaoId2),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio3 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio3),
                    QuestaoId = Convert.ToInt32(QuestaoId3),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio4 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio4),
                    QuestaoId = Convert.ToInt32(QuestaoId4),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio5 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio5),
                    QuestaoId = Convert.ToInt32(QuestaoId5),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio6 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio6),
                    QuestaoId = Convert.ToInt32(QuestaoId6),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio7 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio7),
                    QuestaoId = Convert.ToInt32(QuestaoId7),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio8 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio8),
                    QuestaoId = Convert.ToInt32(QuestaoId8),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio9 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio9),
                    QuestaoId = Convert.ToInt32(QuestaoId9),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio10 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio10),
                    QuestaoId = Convert.ToInt32(QuestaoId10),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio11 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio11),
                    QuestaoId = Convert.ToInt32(QuestaoId11),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio12 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio12),
                    QuestaoId = Convert.ToInt32(QuestaoId12),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio13 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio13),
                    QuestaoId = Convert.ToInt32(QuestaoId13),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio14 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio14),
                    QuestaoId = Convert.ToInt32(QuestaoId14),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio15 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio15),
                    QuestaoId = Convert.ToInt32(QuestaoId15),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio16 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio16),
                    QuestaoId = Convert.ToInt32(QuestaoId16),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio17 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio17),
                    QuestaoId = Convert.ToInt32(QuestaoId17),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio18 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio18),
                    QuestaoId = Convert.ToInt32(QuestaoId18),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio19 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio19),
                    QuestaoId = Convert.ToInt32(QuestaoId19),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            if (Radio20 != null)
            {
                AlunoQuestao aluno = new AlunoQuestao
                {
                    DataHora = DateTime.Now,
                    Nota = Convert.ToDouble(Radio20),
                    QuestaoId = Convert.ToInt32(QuestaoId20),
                    PessoaId = Aluno,
                    AvaliadorId = avaliador
                };
                bd.AlunoQuestao.Add(aluno);
                bd.SaveChanges();
            }
            return RedirectToAction("ResultadoIndividual", new { Aluno, AvaliacaoId });

        }

        [Authorize]
        public ActionResult ResultadoIndividual(int Aluno, int AvaliacaoId)
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
            ViewBag.Aluno = bd.Pessoa.FirstOrDefault(x => x.PessoaId == Aluno);

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