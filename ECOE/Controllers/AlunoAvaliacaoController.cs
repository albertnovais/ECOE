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
            //if (bd.Avaliacoes.FirstOrDefault(x => x.AvaliacaoId == AvaliacaoId).Dupla == true)
            //    return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });

            var alunoQuestao1 = bd.AlunoQuestao.Where(x => x.Questao.AvaliacaoId == AvaliacaoId && x.PessoaId == Pessoa1).ToList().Count();
            var alunoQuestao2 = bd.AlunoQuestao.Where(x => x.Questao.AvaliacaoId == AvaliacaoId && x.PessoaId == Pessoa2).ToList().Count();

            if (alunoQuestao1 > 0 && alunoQuestao2 > 0)
                return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });
            else if (alunoQuestao1 > 0)
                return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });
            else if (alunoQuestao2 > 0)
                return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });

            ViewBag.Mensagem = Mensagem;
            var questoes = bd.Questao.Where(x => x.AvaliacaoId == AvaliacaoId);
            var avaliado = bd.Pessoa.FirstOrDefault(x => x.PessoaId == Pessoa1).Nome;
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
        public ActionResult Avaliar(int AvaliacaoId, int Aluno1, int? Aluno2, string Radio,
            int? QuestaoId1, int? QuestaoId2, int? QuestaoId3, int? QuestaoId4, int? QuestaoId5, int? QuestaoId6, int? QuestaoId7, int? QuestaoId8, int? QuestaoId9, int? QuestaoId10,
            int? QuestaoId11, int? QuestaoId12, int? QuestaoId13, int? QuestaoId14, int? QuestaoId15, int? QuestaoId16, int? QuestaoId17, int? QuestaoId18, int? QuestaoId19, int? QuestaoId20,
            int? QuestaoId21, int? QuestaoId22, int? QuestaoId23, int? QuestaoId24, int? QuestaoId25, int? QuestaoId26, int? QuestaoId27, int? QuestaoId28, int? QuestaoId29, int? QuestaoId30,
            int? Radio1, int? Radio2, int? Radio3, int? Radio4, int? Radio5, int? Radio6, int? Radio7, int? Radio8, int? Radio9, int? Radio10,
            int? Radio11, int? Radio12, int? Radio13, int? Radio14, int? Radio15, int? Radio16, int? Radio17, int? Radio18, int? Radio19, int? Radio20,
            int? Radio21, int? Radio22, int? Radio23, int? Radio24, int? Radio25, int? Radio26, int? Radio27, int? Radio28, int? Radio29, int? Radio30)
        {
           
            var radios = Radio.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            
            var avaliador = Convert.ToInt32(HttpContext.User.Identity.Name);
            if (bd.AlunoQuestao.FirstOrDefault(x => x.QuestaoId == QuestaoId1 && x.PessoaId == Aluno1) != null) return RedirectToAction("AlunoExistente", new { AvaliacaoId, Mensagem = "3" });
            if (Radio1 != null)
            {
                AdicionarAlunoQuestao(Radio1, QuestaoId1, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio1, QuestaoId1, Aluno2);
            }
            if (Radio2 != null)
            {
                AdicionarAlunoQuestao(Radio2, QuestaoId2, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio2, QuestaoId2, Aluno2);
            }
            if (Radio3 != null)
            {
                AdicionarAlunoQuestao(Radio3, QuestaoId3, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio3, QuestaoId3, Aluno2);
            }
            if (Radio4 != null)
            {
                AdicionarAlunoQuestao(Radio4, QuestaoId4, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio4, QuestaoId4, Aluno2);
            }
            if (Radio5 != null)
            {
                AdicionarAlunoQuestao(Radio5, QuestaoId5, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio5, QuestaoId5, Aluno2);
            }
            if (Radio6 != null)
            {
                AdicionarAlunoQuestao(Radio6, QuestaoId6, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio6, QuestaoId6, Aluno2);
            }
            if (Radio7 != null)
            {
                AdicionarAlunoQuestao(Radio7, QuestaoId7, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio7, QuestaoId7, Aluno2);
            }
            if (Radio8 != null)
            {
                AdicionarAlunoQuestao(Radio8, QuestaoId8, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio8, QuestaoId8, Aluno2);
            }
            if (Radio9 != null)
            {
                AdicionarAlunoQuestao(Radio9, QuestaoId9, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio9, QuestaoId9, Aluno2);
            }
            if (Radio10 != null)
            {
                AdicionarAlunoQuestao(Radio10, QuestaoId10, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio10, QuestaoId10, Aluno2);
            }
            if (Radio11 != null)
            {
                AdicionarAlunoQuestao(Radio11, QuestaoId11, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio11, QuestaoId11, Aluno2);
            }
            if (Radio12 != null)
            {
                AdicionarAlunoQuestao(Radio12, QuestaoId12, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio12, QuestaoId12, Aluno2);
            }
            if (Radio13 != null)
            {
                AdicionarAlunoQuestao(Radio13, QuestaoId13, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio13, QuestaoId13, Aluno2);
            }
            if (Radio14 != null)
            {
                AdicionarAlunoQuestao(Radio14, QuestaoId14, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio14, QuestaoId14, Aluno2);
            }
            if (Radio15 != null)
            {
                AdicionarAlunoQuestao(Radio15, QuestaoId15, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio15, QuestaoId15, Aluno2);
            }
            if (Radio16 != null)
            {
                AdicionarAlunoQuestao(Radio16, QuestaoId16, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio16, QuestaoId16, Aluno2);
            }
            if (Radio17 != null)
            {
                AdicionarAlunoQuestao(Radio17, QuestaoId17, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio17, QuestaoId17, Aluno2);
            }
            if (Radio18 != null)
            {
                AdicionarAlunoQuestao(Radio18, QuestaoId18, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio18, QuestaoId18, Aluno2);
            }
            if (Radio19 != null)
            {
                AdicionarAlunoQuestao(Radio19, QuestaoId19, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio19, QuestaoId19, Aluno2);
            }
            if (Radio20 != null)
            {
                AdicionarAlunoQuestao(Radio20, QuestaoId20, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio20, QuestaoId20, Aluno2);
            }
            if (Radio21 != null)
            {
                AdicionarAlunoQuestao(Radio21, QuestaoId21, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio21, QuestaoId21, Aluno2);
            }
            if (Radio22 != null)
            {
                AdicionarAlunoQuestao(Radio22, QuestaoId22, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio22, QuestaoId22, Aluno2);
            }
            if (Radio23 != null)
            {
                AdicionarAlunoQuestao(Radio23, QuestaoId23, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio23, QuestaoId23, Aluno2);
            }
            if (Radio24 != null)
            {
                AdicionarAlunoQuestao(Radio24, QuestaoId24, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio24, QuestaoId24, Aluno2);
            }
            if (Radio25 != null)
            {
                AdicionarAlunoQuestao(Radio25, QuestaoId25, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio25, QuestaoId25, Aluno2);
            }
            if (Radio26 != null)
            {
                AdicionarAlunoQuestao(Radio26, QuestaoId26, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio26, QuestaoId26, Aluno2);
            }
            if (Radio27 != null)
            {
                AdicionarAlunoQuestao(Radio27, QuestaoId27, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio27, QuestaoId27, Aluno2);
            }
            if (Radio28 != null)
            {
                AdicionarAlunoQuestao(Radio28, QuestaoId28, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio28, QuestaoId28, Aluno2);
            }
            if (Radio29 != null)
            {
                AdicionarAlunoQuestao(Radio29, QuestaoId29, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio29, QuestaoId29, Aluno2);
            }
            if (Radio30 != null)
            {
                AdicionarAlunoQuestao(Radio30, QuestaoId30, Aluno1);
                if (Aluno2 != null)
                    AdicionarAlunoQuestao(Radio30, QuestaoId30, Aluno2);
            }
            int h = 2;
            if (1 == h)
            {
                //if (Radio1 != null)
                //{
                //    AlunoQuestao aluno1 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio1),
                //        QuestaoId = Convert.ToInt32(QuestaoId1),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno1);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio1),
                //        QuestaoId = Convert.ToInt32(QuestaoId1),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio2 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio2),
                //        QuestaoId = Convert.ToInt32(QuestaoId2),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio2),
                //        QuestaoId = Convert.ToInt32(QuestaoId2),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio3 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio3),
                //        QuestaoId = Convert.ToInt32(QuestaoId3),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio3),
                //        QuestaoId = Convert.ToInt32(QuestaoId3),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio4 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio4),
                //        QuestaoId = Convert.ToInt32(QuestaoId4),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio4),
                //        QuestaoId = Convert.ToInt32(QuestaoId4),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio5 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio5),
                //        QuestaoId = Convert.ToInt32(QuestaoId5),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio5),
                //        QuestaoId = Convert.ToInt32(QuestaoId5),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio6 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio6),
                //        QuestaoId = Convert.ToInt32(QuestaoId6),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio6),
                //        QuestaoId = Convert.ToInt32(QuestaoId6),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio7 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio7),
                //        QuestaoId = Convert.ToInt32(QuestaoId7),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio7),
                //        QuestaoId = Convert.ToInt32(QuestaoId7),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio8 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio8),
                //        QuestaoId = Convert.ToInt32(QuestaoId8),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio8),
                //        QuestaoId = Convert.ToInt32(QuestaoId8),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio9 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio9),
                //        QuestaoId = Convert.ToInt32(QuestaoId9),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio9),
                //        QuestaoId = Convert.ToInt32(QuestaoId9),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio10 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio10),
                //        QuestaoId = Convert.ToInt32(QuestaoId10),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio10),
                //        QuestaoId = Convert.ToInt32(QuestaoId10),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio11 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio11),
                //        QuestaoId = Convert.ToInt32(QuestaoId11),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio11),
                //        QuestaoId = Convert.ToInt32(QuestaoId11),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio12 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio12),
                //        QuestaoId = Convert.ToInt32(QuestaoId12),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio12),
                //        QuestaoId = Convert.ToInt32(QuestaoId12),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio13 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio13),
                //        QuestaoId = Convert.ToInt32(QuestaoId13),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio13),
                //        QuestaoId = Convert.ToInt32(QuestaoId13),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio14 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio14),
                //        QuestaoId = Convert.ToInt32(QuestaoId14),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio14),
                //        QuestaoId = Convert.ToInt32(QuestaoId14),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio15 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio15),
                //        QuestaoId = Convert.ToInt32(QuestaoId15),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio15),
                //        QuestaoId = Convert.ToInt32(QuestaoId15),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio16 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio16),
                //        QuestaoId = Convert.ToInt32(QuestaoId16),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio16),
                //        QuestaoId = Convert.ToInt32(QuestaoId16),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio17 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio17),
                //        QuestaoId = Convert.ToInt32(QuestaoId17),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio17),
                //        QuestaoId = Convert.ToInt32(QuestaoId17),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio18 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio18),
                //        QuestaoId = Convert.ToInt32(QuestaoId18),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio18),
                //        QuestaoId = Convert.ToInt32(QuestaoId18),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio19 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio19),
                //        QuestaoId = Convert.ToInt32(QuestaoId19),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio19),
                //        QuestaoId = Convert.ToInt32(QuestaoId19),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
                //if (Radio20 != null)
                //{
                //    AlunoQuestao aluno = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio20),
                //        QuestaoId = Convert.ToInt32(QuestaoId20),
                //        PessoaId = Aluno1,
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno);
                //    AlunoQuestao aluno2 = new AlunoQuestao
                //    {
                //        DataHora = DateTime.Now,
                //        Nota = Convert.ToDouble(Radio20),
                //        QuestaoId = Convert.ToInt32(QuestaoId20),
                //        PessoaId = Convert.ToInt32(Aluno2),
                //        AvaliadorId = avaliador
                //    };
                //    bd.AlunoQuestao.Add(aluno2);
                //    bd.SaveChanges();
                //}
            }

            if(Aluno2!= null)          
                return RedirectToAction("ResultadoIndividual", new { Aluno1,Aluno2, AvaliacaoId });
            else
                return RedirectToAction("ResultadoIndividual", new { Aluno = Aluno1,  AvaliacaoId });


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