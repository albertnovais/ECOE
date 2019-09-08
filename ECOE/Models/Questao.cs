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
    
    public partial class Questao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Questao()
        {
            this.AlunoQuestao = new HashSet<AlunoQuestao>();
        }
    
        public int QuestaoId { get; set; }
        public string Descricao { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public int PessoaId { get; set; }
        public int StatusId { get; set; }
        public int AvaliacaoId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlunoQuestao> AlunoQuestao { get; set; }
        public virtual Avaliacoes Avaliacoes { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual Status Status { get; set; }
    }
}
