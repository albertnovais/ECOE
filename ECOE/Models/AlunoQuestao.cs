//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ECOE.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AlunoQuestao
    {
        public int PessoaId { get; set; }
        public System.DateTime DataHora { get; set; }
        public double Nota { get; set; }
        public int QuestaoId { get; set; }
        public Nullable<int> AvaliadorId { get; set; }
    
        public virtual Questao Questao { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual Pessoa Pessoa1 { get; set; }
    }
}
