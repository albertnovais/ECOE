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
    using System.ComponentModel.DataAnnotations;
    
    public partial class Avaliacoes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Avaliacoes()
        {
            this.AlunoAvaliacao = new HashSet<AlunoAvaliacao>();
            this.Questao = new HashSet<Questao>();
        }

        
        public int AvaliacaoId { get; set; }
        public int PessoaId { get; set; }
        [Required(ErrorMessage = "Este Campo � Obrigat�rio.")]
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public int TurmaId { get; set; }
        public Nullable<decimal> Peso { get; set; }
        public Nullable<int> StatusId { get; set; }
        [Required(ErrorMessage = "Este Campo � Obrigat�rio.")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DataAvaliacao { get; set; }
        public bool Dupla { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlunoAvaliacao> AlunoAvaliacao { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual Status Status { get; set; }
        public virtual Turma Turma { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Questao> Questao { get; set; }
    }
}
